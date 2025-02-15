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

public class CollectionLoad32 : BaseCollectionLoad<Key32>
{
    private uint _layoutPointer;
    private Key32[] _types;
    private AttribEntry32[] _entries;

    private long _srcLayoutPtr;
    private long _dstLayoutPtr;

    public override void Read(VaultReadContext<Key32> context, BinaryReader br)
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

        Collection = new VltCollection<Key32>(context.Vault, context.Database.FindClass(new Key32(mClass)),
            new Key32(mKey));

        _types = new Key32[mNumTypes];
        for (var i = 0; i < mNumTypes; i++)
        {
            _types[i] = Key32.Read(br);
        }

        _entries = new AttribEntry32[mNumEntries];

        for (var i = 0; i < mNumEntries; i++)
        {
            var attribEntry = new AttribEntry32(Collection);
            attribEntry.Read(context, br);
            _entries[i] = attribEntry;
        }

        ParentKey = new Key32(mParent);
        context.Database.RowManager.AddCollection(Collection);
    }

    public override void Prepare(Vault<Key32> vault)
    {
        List<KeyValuePair<Key32, object>> optionalDataColumns = (from pair in Collection.GetData()
            where !Collection.Class[pair.Key].IsInLayout
            select pair).ToList();

        _entries = new AttribEntry32[optionalDataColumns.Count];
        _types = Collection.Class.BaseFields.Select(f => f.TypeKey)
            .Concat(optionalDataColumns.Select(c => Collection.Class[c.Key].TypeKey))
            .Distinct().ToArray();

        for (var index = 0; index < optionalDataColumns.Count; index++)
        {
            var optionalDataColumn = optionalDataColumns[index];
            var entry = new AttribEntry32(Collection);

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
                entry.InlineData = new VltAttribType<Key32>
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

    public override void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
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

    public override Key32 GetExportId()
    {
        // TODO: the collection should probably have an ID separate from key.
        return new Key32((uint)HashCode.Combine(Collection.Class.Key, Collection.Key));
    }

    public override void ReadPointerData(VaultReadContext<Key32> context, BinaryReader br)
    {
        if (_layoutPointer != 0)
        {
            br.BaseStream.Position = _layoutPointer;

            var maxAlignment = Collection.Class.BaseFields.Max(f => f.Alignment);
            Debug.Assert(_layoutPointer % maxAlignment == 0);

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, baseField, Collection);
                br.SafeAlignReader(baseField.Alignment);

                if (br.BaseStream.Position - _layoutPointer != baseField.Offset)
                {
                    throw new Exception(
                        $"trying to read field {baseField.Key} at offset 0x{br.BaseStream.Position - _layoutPointer:X}, need to be at 0x{baseField.Offset:X}");
                }

                var valueStartPos = br.BaseStream.Position;
                var rawValue =
                    context.Database.TypeRegistry.ReadFieldValue(context,
                        fieldContext, br);
                var valueEndPos = br.BaseStream.Position;

                var valueBytesRead = valueEndPos - valueStartPos;

                var expectedDataSize = GetExpectedDataSize(baseField, rawValue, valueStartPos);
                var type = rawValue.GetType();
                Debug.Assert(valueBytesRead == expectedDataSize,
                    "valueBytesRead == GetExpectedDataSize(baseField, rawValue, valueStartPos)",
                    $"Read {valueBytesRead} bytes for type {type}, expected to read {expectedDataSize}");

                Collection.SetRawValue(baseField.Key, rawValue);
            }

            var layoutBytesRead = br.BaseStream.Position - _layoutPointer;

            if (layoutBytesRead > Collection.Class.LayoutSize)
            {
                throw new Exception("read too much layout data");
            }
        }

        foreach (var entry in _entries)
        {
            var optionalField = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, optionalField, Collection);

            if ((optionalField.Flags & DefinitionFlags.IsStatic) != 0)
            {
                throw new Exception("Encountered static field as an entry. Processing will not continue.");
            }

            if ((optionalField.Flags & DefinitionFlags.Array) != 0)
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsArray) ==
                             NodeFlagsEnum.IsArray);
            }
            else
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsArray) == 0);
            }

            if (entry.InlineData is VltAttribType<Key32> attribType)
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) == 0);
                attribType.ReadPointerData(context, fieldContext, br);
                Collection.SetRawValue(optionalField.Key, attribType.Data);
            }
            else
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) ==
                             NodeFlagsEnum.IsInline);
                Collection.SetRawValue(optionalField.Key, entry.InlineData);
            }
        }

        foreach (var dataEntry in Collection.GetData())
        {
            var fieldContext =
                new FieldReadWriteContext<Key32>(Collection.Class, Collection.Class[dataEntry.Key], Collection);
            if (dataEntry.Value is IVltPointerObject<Key32> vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }
    }

    public override void WritePointerData(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        // Part 1: write base fields (layout)
        if (Collection.Class.HasBaseFields)
        {
            var maxAlignment = Collection.Class.BaseFields.Max(f => f.Alignment);
            bw.AlignWriter(maxAlignment);

            _dstLayoutPtr = bw.BaseStream.Position;

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, baseField, Collection);

                bw.BaseStream.Position = _dstLayoutPtr + baseField.Offset;

                var rawValue = Collection.GetRawValue(baseField.Key);
                var valueStartPos = bw.BaseStream.Position;
                context.Database.TypeRegistry.WriteFieldValue(rawValue, context, fieldContext, bw);
                var valueEndPos = bw.BaseStream.Position;
                var valueBytesWritten = valueEndPos - valueStartPos;

                Debug.Assert(valueBytesWritten == GetExpectedDataSize(baseField, rawValue, valueStartPos),
                    "valueBytesWritten == GetExpectedDataSize(baseField, rawValue, valueStartPos)");
            }
        }

        // Part 2: Write non-inline optional fields
        foreach (var entry in _entries)
        {
            if (entry.InlineData is not VltAttribType<Key32> attrib)
            {
                continue;
            }

            var field = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, field, Collection);

            attrib.WritePointerData(context, fieldContext, bw);
        }

        // Part 3: Write pointer data for all fields
        foreach (var entry in Collection.GetOrderedData())
        {
            var field = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, field, Collection);
            if (entry.Value is IVltPointerObject<Key32> vltPointerObject)
            {
                vltPointerObject.WritePointerData(context, fieldContext, bw);
            }
        }
    }

    public override void AddPointers(VaultWriteContext<Key32> context)
    {
        context.AddPointer(_srcLayoutPtr, _dstLayoutPtr, true);

        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext<Key32>(Collection.Class, baseField, Collection);
            var rawValue = Collection.GetRawValue(baseField.Key);

            if (rawValue is IVltPointerObject<Key32> vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }

        foreach (var entry in _entries)
        {
            var fieldContext =
                new FieldReadWriteContext<Key32>(Collection.Class, Collection.Class[entry.Key], Collection);
            if (entry.InlineData is IVltPointerObject<Key32> vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }
    }

    private static long GetExpectedDataSize(VltClassField<Key32> field, object value, long offset)
    {
        if (!field.IsArray)
        {
            return field.Size;
        }

        var array = (VltArrayType<Key32>)value;
        var dataStartPos = offset + 8;
        var alignmentOffset = field.Alignment - 1;
        var alignedDataStartPos = (dataStartPos + alignmentOffset) & ~alignmentOffset;

        return (alignedDataStartPos - offset) + field.Size * array.Capacity;
    }
}