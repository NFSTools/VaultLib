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

public class CollectionLoad : ModernCollectionLoadBase<Key32, AttribEntry32>
{
    public override void Read(VaultReadContext<Key32> context, BinaryReader br)
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

        // Debug.Assert(LayoutPointer % 4 == 0, "LayoutPointer % 4 == 0");

        Debug.Assert(mTableReserve == mNumEntries);

        Collection = new VltCollection<Key32>(context.Vault, context.Database.FindClass(new Key32(mClass)),
            new Key32(mKey));

        Debug.Assert(mTypesLen >= mNumTypes);

        Types = new Key32[mNumTypes];
        for (var i = 0; i < mNumTypes; i++)
        {
            Types[i] = Key32.Read(br);
        }

        for (var i = 0; i < mTypesLen - mNumTypes; i++)
        {
            br.ReadUInt32();
        }

        Entries = new List<AttribEntry32>();

        for (var i = 0; i < mNumEntries; i++)
        {
            var attribEntry = new AttribEntry32(Collection);

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

        ParentKey = new Key32(mParent);
        context.Database.RowManager.AddCollection(Collection);
    }

    public override void Prepare(Vault<Key32> vault)
    {
        List<KeyValuePair<Key32, object>> optionalDataColumns = (from pair in Collection.GetOrderedData()
            let field = Collection.Class[pair.Key]
            where !field.IsInLayout
            select new KeyValuePair<Key32, object>(pair.Key, pair.Value)).ToList();

        Entries = new List<AttribEntry32>();
        Types = Collection.Class.BaseFields.Select(f => f.TypeKey)
            .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeKey))
            .Distinct().ToArray();

        for (var index = 0; index < optionalDataColumns.Count; index++)
        {
            var optionalDataColumn = optionalDataColumns[index];
            var entry = new AttribEntry32(Collection);
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
                    new VltAttribType<Key32>()
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

    public override void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
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

        foreach (var type in Types)
        {
            type.Write(bw);
        }

        if (typesLen != numTypes)
            bw.Write(0);

        foreach (var attribEntry in Entries)
        {
            attribEntry.Write(context, bw);
        }
    }

    public override Key32 GetExportId()
    {
        // TODO: the collection should probably have an ID separate from key.
        return new Key32((uint)HashCode.Combine(Collection.Class.Key, Collection.Key));
    }
}