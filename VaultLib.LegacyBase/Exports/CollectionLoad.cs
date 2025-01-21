// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:19 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports;

public class CollectionLoad : BaseCollectionLoad
{
    private uint _layoutPointer;
    private uint[] _types;
    private AttribEntry[] _entries;

    private long _srcLayoutPtr;
    private long _dstLayoutPtr;

    public override void Read(VaultReadContext context, BinaryReader br)
    {
        var mKey = br.ReadUInt32(); // 4
        var mClass = br.ReadUInt32(); // 8
        var mParent = br.ReadUInt32(); // 12
        var mTableReserve = br.ReadUInt32(); // 16
        br.ReadUInt32(); // 20
        var mNumEntries = br.ReadUInt32(); // 24
        var mNumTypes = br.ReadUInt32(); // 28
        _layoutPointer = br.ReadPointer(); // 32

        Debug.Assert(mTableReserve == mNumEntries);

        Collection = new VltCollection(context.Vault, context.Database.FindClass(HashManager.ResolveVlt(mClass)),
            HashManager.ResolveVlt(mKey));

        _types = new uint[mNumTypes];
        for (var i = 0; i < mNumTypes; i++)
        {
            _types[i] = (br.ReadUInt32());
        }

        _entries = new AttribEntry[mNumEntries];

        for (var i = 0; i < mNumEntries; i++)
        {
            var attribEntry = new AttribEntry(Collection);
            attribEntry.Read(context, br);
            _entries[i] = attribEntry;
        }

        ParentKey = mParent;
        context.Database.RowManager.AddCollection(Collection);
    }

    public override void Prepare(Vault vault)
    {
        List<KeyValuePair<string, object>> optionalDataColumns = (from pair in Collection.GetData()
            where !Collection.Class[pair.Key].IsInLayout
            select pair).ToList();

        _entries = new AttribEntry[optionalDataColumns.Count];
        _types = Collection.Class.BaseFields.Select(f => f.TypeName)
            .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeName))
            .Select(s => Vlt32Hasher.Hash(s)).Distinct().ToArray();

        for (var index = 0; index < optionalDataColumns.Count; index++)
        {
            var optionalDataColumn = optionalDataColumns[index];
            var entry = new AttribEntry(Collection);

            entry.Key = Vlt32Hasher.Hash(optionalDataColumn.Key);
            var vltClassField = Collection.Class[optionalDataColumn.Key];
            entry.TypeIndex = (ushort)Array.IndexOf(_types,
                Vlt32Hasher.Hash(vltClassField.TypeName));
            entry.NodeFlags = NodeFlagsEnum.Default;

            if (entry.IsInline())
            {
                entry.InlineData = optionalDataColumn.Value;
                entry.NodeFlags |= NodeFlagsEnum.IsInline;
            }
            else
            {
                entry.InlineData = new VltAttribType()
                {
                    Data = optionalDataColumn.Value
                };
            }

            if (vltClassField.IsArray)
            {
                entry.NodeFlags |= NodeFlagsEnum.IsArray;
            }

            _entries[index] = entry;
        }
    }

    public override void Write(VaultWriteContext context, BinaryWriter bw)
    {
        bw.Write(Vlt32Hasher.Hash(Collection.Name));
        bw.Write(Vlt32Hasher.Hash(Collection.Class.Name));
        //bw.Write((uint) (Collection.Parent?.Key ?? 0));
        bw.Write(Collection.Parent != null ? Vlt32Hasher.Hash(Collection.Parent.Name) : 0u);
        bw.Write((uint)_entries.Length);
        bw.Write(0);
        bw.Write((uint)_entries.Length);
        bw.Write((uint)_types.Length);
        _srcLayoutPtr = bw.BaseStream.Position;
        bw.Write(0);

        foreach (var type in _types)
        {
            bw.Write(type);
        }

        foreach (var entry in _entries)
        {
            entry.Write(context, bw);
        }
    }

    public override ulong GetExportId()
    {
        return Vlt32Hasher.Hash($"{Collection.Class.Name}/{Collection.Name}");
    }

    public override void ReadPointerData(VaultReadContext context, BinaryReader br)
    {
        if (_layoutPointer != 0)
        {
            br.BaseStream.Position = _layoutPointer;

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
                br.AlignReader(baseField.Alignment);

                long startPos = br.BaseStream.Position;
                var data =
                    context.Database.TypeRegistry.ReadFieldValue(context,
                        fieldContext, br);
                long endPos = br.BaseStream.Position;
                if (!baseField.IsArray && endPos - startPos != baseField.Size)
                {
                    throw new Exception($"read {endPos - startPos} bytes, needed to read {baseField.Size}");
                }

                //Collection.Data[baseField.Name] = data;
                Collection.SetRawValue(baseField.Name, data);
            }
        }

        foreach (var entry in _entries)
        {
            var optionalField = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext(Collection.Class, optionalField, Collection);

            if ((optionalField.Flags & DefinitionFlags.IsStatic) != 0)
            {
                throw new Exception(
                    "Congratulations. You have successfully broken this library. Please consult with your doctor for further instructions.");
            }

            if (entry.InlineData is VltAttribType attribType)
            {
                attribType.ReadPointerData(context, fieldContext, br);
                Collection.SetRawValue(optionalField.Name, attribType.Data);
                //Collection.Data[optionalField.Name] = attribType.Data;
            }
            else
            {
                Collection.SetRawValue(optionalField.Name, entry.InlineData);
                //Collection.Data[optionalField.Name] = entry.InlineData;
            }
        }

        foreach (var dataEntry in Collection.GetData())
        {
            var fieldContext =
                new FieldReadWriteContext(Collection.Class, Collection.Class[dataEntry.Key], Collection);
            if (dataEntry.Value is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }
    }

    public override void WritePointerData(VaultWriteContext context, BinaryWriter bw)
    {
        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
            bw.AlignWriter(baseField.Alignment);
            if (_dstLayoutPtr == 0)
            {
                _dstLayoutPtr = bw.BaseStream.Position;
            }

            if (bw.BaseStream.Position - _dstLayoutPtr != baseField.Offset)
            {
                throw new Exception("incorrect offset");
            }

            var rawValue = Collection.GetRawValue(baseField.Name);
            context.Database.TypeRegistry.WriteFieldValue(rawValue, context, fieldContext, bw);
        }

        foreach (var dataPair in Collection.GetData())
        {
            VltClassField field = Collection.Class[dataPair.Key];
            var fieldContext = new FieldReadWriteContext(Collection.Class, field, Collection);

            if (!field.IsInLayout)
            {
                var entry = _entries.First(e => e.Key == field.Key);

                if (entry.InlineData is IVltPointerObject vltPointerObject)
                {
                    bw.AlignWriter(field.Alignment);
                    vltPointerObject.WritePointerData(context, fieldContext, bw);
                }
            }
            else
            {
                if (dataPair.Value is IVltPointerObject vltPointerObject)
                {
                    bw.AlignWriter(field.Alignment);
                    vltPointerObject.WritePointerData(context, fieldContext, bw);
                }
            }
        }

        bw.AlignWriter(Collection.Class.HasBaseFields ? 4 : 2);
    }

    public override void AddPointers(VaultWriteContext context)
    {
        context.AddPointer(_srcLayoutPtr, _dstLayoutPtr, true);

        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
            var rawValue = Collection.GetRawValue(baseField.Name);

            if (rawValue is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }

        foreach (var entry in _entries)
        {
            var fieldContext = new FieldReadWriteContext(Collection.Class, Collection.Class[entry.Key], Collection);
            if (entry.InlineData is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }
    }
}