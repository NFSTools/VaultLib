// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:19 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.LegacyBase.Exports;

public class CollectionLoad64 : BaseCollectionLoad<Key64>
{
    private uint _layoutPointer;
    private Key64[] _types;
    private AttribEntry64[] _entries;

    private long _srcLayoutPtr;
    private long _dstLayoutPtr;

    public override void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        var mKey = br.ReadUInt64();
        var mClass = br.ReadUInt64();
        var mParent = br.ReadUInt64();
        var mTableReserve = br.ReadUInt32();
        br.ReadUInt32();
        var mNumEntries = br.ReadUInt32();
        var mNumTypes = br.ReadUInt32();
        _layoutPointer = br.ReadPointer();

        // NOTE: This is an artifact of structure alignment.
        br.ReadInt32();

        Debug.Assert(mTableReserve == mNumEntries);

        Collection = new VltCollection<Key64>(context.Vault, context.Database.FindClass(new Key64(mClass)),
            new Key64(mKey));

        _types = new Key64[mNumTypes];
        for (var i = 0; i < mNumTypes; i++)
        {
            _types[i] = Key64.Read(br);
        }

        _entries = new AttribEntry64[mNumEntries];

        for (var i = 0; i < mNumEntries; i++)
        {
            var attribEntry = new AttribEntry64(Collection);
            attribEntry.Read(context, br);
            _entries[i] = attribEntry;
        }

        ParentKey = new Key64(mParent);
        context.Database.RowManager.AddCollection(Collection);
    }

    public override void Prepare(Vault<Key64> vault)
    {
        List<KeyValuePair<Key64, object>> optionalDataColumns = (from pair in Collection.GetData()
            where !Collection.Class[pair.Key].IsInLayout
            select pair).ToList();

        _entries = new AttribEntry64[optionalDataColumns.Count];
        _types = Collection.Class.BaseFields.Select(f => f.TypeKey)
            .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeKey))
            .Distinct().ToArray();

        for (var index = 0; index < optionalDataColumns.Count; index++)
        {
            var optionalDataColumn = optionalDataColumns[index];
            var entry = new AttribEntry64(Collection);

            entry.Key = optionalDataColumn.Key;
            var vltClassField = Collection.Class[optionalDataColumn.Key];
            entry.TypeIndex = (ushort)Array.IndexOf(_types,
                vltClassField.TypeKey);
            entry.NodeFlags = NodeFlagsEnum.Default;

            if (entry.IsInline())
            {
                entry.InlineData = optionalDataColumn.Value;
                entry.NodeFlags |= NodeFlagsEnum.IsInline;
            }
            else
            {
                entry.InlineData = new VltAttribType<Key64>
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

    public override void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.Write(Collection.Key.Hash);
        bw.Write(Collection.Class.Key.Hash);
        bw.Write(Collection.Parent?.Key.Hash ?? 0);
        bw.Write((uint)_entries.Length);
        bw.Write(0);
        bw.Write((uint)_entries.Length);
        bw.Write((uint)_types.Length);
        _srcLayoutPtr = bw.BaseStream.Position;
        bw.Write(0);

        foreach (var type in _types)
        {
            type.Write(bw);
        }

        foreach (var entry in _entries)
        {
            entry.Write(context, bw);
        }
    }

    public override Key64 GetExportId()
    {
        // TODO: the collection should probably have an ID separate from key.
        return new Key64((Collection.Class.Key.Hash | Collection.Key.Hash));
        // return Key64.FromString($"{Collection.Class.Name}/{Collection.Name}");
    }

    public override void ReadPointerData(VaultReadContext<Key64> context, BinaryReader br)
    {
        if (_layoutPointer != 0)
        {
            br.BaseStream.Position = _layoutPointer;

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, baseField, Collection);
                br.SafeAlignReader(baseField.Alignment);

                long startPos = br.BaseStream.Position;
                var data =
                    context.Database.TypeRegistry.ReadFieldValue(context,
                        fieldContext,
                        br);
                long endPos = br.BaseStream.Position;
                if (!baseField.IsArray && endPos - startPos != baseField.Size)
                {
                    throw new Exception($"read {endPos - startPos} bytes, needed to read {baseField.Size}");
                }

                //Collection.Data[baseField.Name] = data;
                Collection.SetRawValue(baseField.Key, data);
            }
        }

        foreach (var entry in _entries)
        {
            var optionalField = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, optionalField, Collection);

            if ((optionalField.Flags & DefinitionFlags.IsStatic) != 0)
            {
                throw new Exception(
                    "Congratulations. You have successfully broken this library. Please consult with your doctor for further instructions.");
            }

            if (entry.InlineData is VltAttribType<Key64> attribType)
            {
                attribType.ReadPointerData(context, fieldContext, br);
                Collection.SetRawValue(optionalField.Key, attribType.Data);
                //Collection.Data[optionalField.Name] = attribType.Data;
            }
            else
            {
                Collection.SetRawValue(optionalField.Key, entry.InlineData);
                //Collection.Data[optionalField.Name] = entry.InlineData;
            }
        }

        foreach (var dataEntry in Collection.GetData())
        {
            var fieldContext =
                new FieldReadWriteContext<Key64>(Collection.Class, Collection.Class[dataEntry.Key], Collection);
            if (dataEntry.Value is IVltPointerObject<Key64> vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }
    }

    public override void WritePointerData(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, baseField, Collection);
            bw.AlignWriter(baseField.Alignment);
            if (_dstLayoutPtr == 0)
            {
                _dstLayoutPtr = bw.BaseStream.Position;
            }

            if (bw.BaseStream.Position - _dstLayoutPtr != baseField.Offset)
            {
                throw new Exception("incorrect offset");
            }

            var rawValue = Collection.GetRawValue(baseField.Key);
            context.Database.TypeRegistry.WriteFieldValue(rawValue, context, fieldContext, bw);
        }

        foreach (var dataPair in Collection.GetData())
        {
            var field = Collection.Class[dataPair.Key];
            var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, field, Collection);

            if (!field.IsInLayout)
            {
                var entry = _entries.First(e => e.Key == field.Key);

                if (entry.InlineData is IVltPointerObject<Key64> vltPointerObject)
                {
                    bw.AlignWriter(field.Alignment);
                    vltPointerObject.WritePointerData(context, fieldContext, bw);
                }
            }
            else
            {
                if (dataPair.Value is IVltPointerObject<Key64> vltPointerObject)
                {
                    bw.AlignWriter(field.Alignment);
                    vltPointerObject.WritePointerData(context, fieldContext, bw);
                }
            }
        }

        bw.AlignWriter(Collection.Class.HasBaseFields ? 4 : 2);
    }

    public override void AddPointers(VaultWriteContext<Key64> context)
    {
        context.AddPointer(_srcLayoutPtr, _dstLayoutPtr, true);

        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext<Key64>(Collection.Class, baseField, Collection);
            var rawValue = Collection.GetRawValue(baseField.Key);

            if (rawValue is IVltPointerObject<Key64> vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }

        foreach (var entry in _entries)
        {
            var fieldContext =
                new FieldReadWriteContext<Key64>(Collection.Class, Collection.Class[entry.Key], Collection);
            if (entry.InlineData is IVltPointerObject<Key64> vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }
    }
}