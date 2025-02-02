using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports;

public class CollectionLoad64 : ModernCollectionLoadBase<Key64, AttribEntry64>
{
    public override void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        var mKey = br.ReadUInt64();
        var mClass = br.ReadUInt64();
        var mParent = br.ReadUInt64();
        var mTableReserve = br.ReadUInt32();
        br.ReadUInt32();
        var mNumEntries = br.ReadUInt32();
        var mNumTypes = br.ReadUInt16();
        var mTypesLen = br.ReadUInt16();
        LayoutPointer = br.ReadPointer();
        br.ReadUInt32();

        Debug.Assert(mTableReserve == mNumEntries);

        Collection = new VltCollection<Key64>(context.Vault, context.Database.FindClass(new Key64(mClass)),
            new Key64(mKey));

        Debug.Assert(mTypesLen >= mNumTypes);

        Types = new Key64[mNumTypes];
        for (var i = 0; i < mNumTypes; i++)
        {
            Types[i] = Key64.Read(br);
        }

        for (var i = 0; i < mTypesLen - mNumTypes; i++)
        {
            br.ReadUInt64();
        }

        Entries = new List<AttribEntry64>();

        for (var i = 0; i < mNumEntries; i++)
        {
            var attribEntry = new AttribEntry64(Collection);

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

        ParentKey = new Key64(mParent);
        context.Database.RowManager.AddCollection(Collection);
    }

    public override void Prepare(Vault<Key64> vault)
    {
        List<KeyValuePair<Key64, object>> optionalDataColumns = (from pair in Collection.GetData()
            let field = Collection.Class[pair.Key]
            where !field.IsInLayout
            select pair).ToList();

        Entries = new List<AttribEntry64>();
        Types = Collection.Class.BaseFields.Select(f => f.TypeKey)
            .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeKey))
            .Distinct().ToArray();

        for (var index = 0; index < optionalDataColumns.Count; index++)
        {
            var optionalDataColumn = optionalDataColumns[index];
            var entry = new AttribEntry64(Collection);
            var vltClassField = Collection.Class[optionalDataColumn.Key];

            entry.Key = optionalDataColumn.Key;
            entry.TypeIndex = (ushort)Array.IndexOf(Types,
                vltClassField.TypeKey);
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
                    new VltAttribType<Key64>
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

    public override void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.Write(Collection.Key.Hash);
        bw.Write(Collection.Class.Key.Hash);
        bw.Write(Collection.Parent?.Key.Hash ?? 0);
        bw.Write(Entries.Count);
        bw.Write(0);
        bw.Write(Entries.Count);

        ushort numTypes = (ushort)Types.Length;
        ushort typesLen = (ushort)(numTypes % 2 == 0 ? numTypes : numTypes + 1);

        bw.Write(numTypes);
        bw.Write(typesLen);
        SourceLayoutPointer = bw.BaseStream.Position;
        bw.Write(0);
        bw.Write(0); // align

        foreach (var type in Types)
        {
            type.Write(bw);
        }

        if (typesLen != numTypes)
            bw.Write(0L);

        foreach (var attribEntry in Entries)
        {
            attribEntry.Write(context, bw);
        }
    }

    public override Key64 GetExportId()
    {
        // TODO: the collection should probably have an ID separate from key.
        return new Key64((Collection.Class.Key.Hash | Collection.Key.Hash));
        // return Key64.FromString($"{Collection.Class.Name}/{Collection.Name}");
    }
}