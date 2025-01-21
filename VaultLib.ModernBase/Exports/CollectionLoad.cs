using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports;

public class CollectionLoad : ModernCollectionLoadBase<AttribEntry>
{
    public override void Read(VaultReadContext context, BinaryReader br)
    {
        var mKey = br.ReadUInt32();
        var mClass = br.ReadUInt32();
        var mParent = br.ReadUInt32();
        var mTableReserve = br.ReadUInt32();
        br.ReadUInt32();
        var mNumEntries = br.ReadUInt32();
        var mNumTypes = br.ReadUInt16();
        var mTypesLen = br.ReadUInt16();
        LayoutPointer = br.ReadPointer();

        Debug.Assert(mTableReserve == mNumEntries);

        Collection = new VltCollection(context.Vault, context.Database.FindClass(HashManager.ResolveVlt(mClass)),
            HashManager.ResolveVlt(mKey));

        Debug.Assert(mTypesLen >= mNumTypes);

        Types = new uint[mNumTypes];
        for (var i = 0; i < mNumTypes; i++)
        {
            Types[i] = (br.ReadUInt32());
        }

        for (var i = 0; i < mTypesLen - mNumTypes; i++)
        {
            br.ReadUInt32();
        }

        Entries = new List<AttribEntry>();

        for (var i = 0; i < mNumEntries; i++)
        {
            var attribEntry = new AttribEntry(Collection);

            attribEntry.Read(context, br);

            // save pos
            long pos = br.BaseStream.Position;
            var readData = attribEntry.ReadData(context, br);
            br.BaseStream.Position = pos;

            if (!readData)
            {
                continue;
            }

            Entries.Add(attribEntry);
        }

        ParentKey = mParent;
        context.Database.RowManager.AddCollection(Collection);
    }

    public override void Prepare(Vault vault)
    {
        List<KeyValuePair<string, object>> optionalDataColumns = (from pair in Collection.GetOrderedData()
            let field = Collection.Class[pair.Key]
            where !field.IsInLayout
            select new KeyValuePair<string, object>(pair.Key, pair.Value)).ToList();

        Entries = new List<AttribEntry>();
        Types = Collection.Class.BaseFields.Select(f => f.TypeName)
            .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeName))
            .Select(s => Vlt32Hasher.Hash(s)).Distinct().ToArray();

        for (var index = 0; index < optionalDataColumns.Count; index++)
        {
            var optionalDataColumn = optionalDataColumns[index];
            var entry = new AttribEntry(Collection);
            var vltClassField = Collection.Class[optionalDataColumn.Key];

            entry.Key = Vlt32Hasher.Hash(optionalDataColumn.Key);
            entry.TypeIndex = (ushort)Array.IndexOf(Types,
                Vlt32Hasher.Hash(vltClassField.TypeName));
            entry.EntryFlags = 0;
            entry.NodeFlags = NodeFlagsEnum.Default;

            if (entry.IsInline())
            {
                entry.InlineData = optionalDataColumn.Value;
                entry.NodeFlags |= NodeFlagsEnum.IsInline;
            }
            else
            {
                entry.InlineData =
                    new VltAttribType()
                        { Data = optionalDataColumn.Value };
            }

            if (vltClassField.IsArray)
            {
                entry.NodeFlags |= NodeFlagsEnum.IsArray;
            }

            if ((vltClassField.Flags & DefinitionFlags.HasHandler) != 0)
            {
                entry.NodeFlags |= NodeFlagsEnum.HasHandler;
            }

            Entries.Add(entry);
        }
    }

    public override void Write(VaultWriteContext context, BinaryWriter bw)
    {
        bw.Write(Vlt32Hasher.Hash(Collection.Name));
        bw.Write(Vlt32Hasher.Hash(Collection.Class.Name));
        bw.Write(Collection.Parent != null ? Vlt32Hasher.Hash(Collection.Parent.Name) : 0u);
        bw.Write(Entries.Count);
        bw.Write(0);
        bw.Write(Entries.Count);

        ushort numTypes = (ushort)Types.Length;
        ushort typesLen = (ushort)(numTypes % 2 == 0 ? numTypes : numTypes + 1);

        bw.Write(numTypes);
        bw.Write(typesLen);
        SourceLayoutPointer = bw.BaseStream.Position;
        bw.Write(0);

        foreach (var type in Types)
        {
            bw.Write(type);
        }

        if (typesLen != numTypes)
            bw.Write(0);

        foreach (var attribEntry in Entries)
        {
            attribEntry.Write(context, bw);
        }
    }

    public override ulong GetExportId()
    {
        return Vlt32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
    }
}