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
using VaultLib.ModernBase.Exports;

namespace VaultLib.ModernBase;

public abstract class ModernCollectionLoadBase<TKey, TAttribEntry> : BaseCollectionLoad<TKey>
    where TAttribEntry : AttribEntryBase<TKey> where TKey : struct, IKey<TKey>
{
    protected uint LayoutPointer { get; set; }

    protected TKey[] Types { get; set; }

    protected List<TAttribEntry> Entries { get; set; }

    protected long SourceLayoutPointer { get; set; }

    private long DestinationLayoutPointer { get; set; }

    public override void ReadPointerData(VaultReadContext<TKey> context, BinaryReader br)
    {
        if (LayoutPointer != 0)
        {
            br.BaseStream.Position = LayoutPointer;

            var maxAlignment = Collection.Class.BaseFields.Max(f => f.Alignment);
            Debug.Assert(LayoutPointer % maxAlignment == 0);
            
            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, baseField, Collection);
                br.SafeAlignReader(baseField.Alignment);

                if (br.BaseStream.Position - LayoutPointer != baseField.Offset)
                {
                    throw new Exception(
                        $"trying to read field {baseField.Key} at offset 0x{br.BaseStream.Position - LayoutPointer:X}, need to be at 0x{baseField.Offset:X}");
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

            var layoutBytesRead = br.BaseStream.Position - LayoutPointer;

            // if (layoutBytesRead > Collection.Class.LayoutSize)
            // {
            //     throw new Exception("read too much layout data");
            // }
        }

        foreach (var entry in Entries)
        {
            var optionalField = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, optionalField, Collection);

            if ((optionalField.Flags & DefinitionFlags.IsStatic) != 0)
            {
                throw new Exception("Encountered static field as an entry. Processing will not continue.");
            }

            if ((optionalField.Flags & DefinitionFlags.HasHandler) != 0)
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.HasHandler) ==
                             NodeFlagsEnum.HasHandler);
            }
            else
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.HasHandler) == 0);
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

            if (entry.InlineData is VltAttribType<TKey> attribType)
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
                new FieldReadWriteContext<TKey>(Collection.Class, Collection.Class[dataEntry.Key], Collection);
            if (dataEntry.Value is IVltPointerObject<TKey> vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }
    }

    public override void WritePointerData(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        // Part 1: write base fields (layout)
        if (Collection.Class.HasBaseFields)
        {
            var maxAlignment = Collection.Class.BaseFields.Max(f => f.Alignment);
            bw.AlignWriter(maxAlignment);
            
            DestinationLayoutPointer = bw.BaseStream.Position;

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, baseField, Collection);

                bw.BaseStream.Position = DestinationLayoutPointer + baseField.Offset;

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
        foreach (var entry in Entries)
        {
            if (entry.InlineData is not VltAttribType<TKey> attrib)
            {
                continue;
            }

            var field = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, field, Collection);

            attrib.WritePointerData(context, fieldContext, bw);
        }

        // Part 3: Write pointer data for all fields
        foreach (var entry in Collection.GetOrderedData())
        {
            var field = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, field, Collection);
            if (entry.Value is IVltPointerObject<TKey> vltPointerObject)
            {
                vltPointerObject.WritePointerData(context, fieldContext, bw);
            }
        }
    }

    public override void AddPointers(VaultWriteContext<TKey> context)
    {
        if (DestinationLayoutPointer != 0)
        {
            context.AddPointer(SourceLayoutPointer, DestinationLayoutPointer, true);
        }

        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, baseField, Collection);
            var rawValue = Collection.GetRawValue(baseField.Key);

            if (rawValue is IVltPointerObject<TKey> vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }

        foreach (var entry in Entries)
        {
            var fieldContext = new FieldReadWriteContext<TKey>(Collection.Class, Collection.Class[entry.Key], Collection);
            if (entry.InlineData is IVltPointerObject<TKey> vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }
    }

    private static long GetExpectedDataSize(VltClassField<TKey> field, object value, long offset)
    {
        if (!field.IsArray)
        {
            return field.Size;
        }

        var array = (VltArrayType<TKey>)value;
        var dataStartPos = offset + 8;
        var alignmentOffset = field.Alignment - 1;
        var alignedDataStartPos = (dataStartPos + alignmentOffset) & ~alignmentOffset;

        return (alignedDataStartPos - offset) + field.Size * array.Capacity;
    }
}