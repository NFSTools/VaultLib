using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.ModernBase.Exports;

namespace VaultLib.ModernBase;

public abstract class ModernCollectionLoadBase<TAttribEntry> : BaseCollectionLoad
    where TAttribEntry : AttribEntryBase
{
    protected uint LayoutPointer { get; set; }

    protected uint[] Types { get; set; }

    protected List<TAttribEntry> Entries { get; set; }

    protected long SourceLayoutPointer { get; set; }

    private long DestinationLayoutPointer { get; set; }

    public override void ReadPointerData(VaultReadContext context, BinaryReader br)
    {
        if (LayoutPointer != 0)
        {
            br.BaseStream.Position = LayoutPointer;

            var maxAlignment = Collection.Class.BaseFields.Max(f => f.Alignment);
            Debug.Assert(LayoutPointer % maxAlignment == 0);
            
            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
                br.SafeAlignReader(baseField.Alignment);

                if (br.BaseStream.Position - LayoutPointer != baseField.Offset)
                {
                    throw new Exception(
                        $"trying to read field {baseField.Name} at offset {br.BaseStream.Position - LayoutPointer:X}, need to be at {baseField.Offset:X}");
                }

                var valueStartPos = br.BaseStream.Position;
                var rawValue =
                    context.Database.TypeRegistry.ReadFieldValue(context,
                        fieldContext, br);
                var valueEndPos = br.BaseStream.Position;

                var valueBytesRead = valueEndPos - valueStartPos;

                Debug.Assert(valueBytesRead == GetExpectedDataSize(baseField, rawValue, valueStartPos),
                    "valueBytesRead == GetExpectedDataSize(baseField, rawValue, valueStartPos)");

                Collection.SetRawValue(baseField.Name, rawValue);
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
            var fieldContext = new FieldReadWriteContext(Collection.Class, optionalField, Collection);

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

            if (entry.InlineData is VltAttribType attribType)
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) == 0);
                attribType.ReadPointerData(context, fieldContext, br);
                Collection.SetRawValue(optionalField.Name, attribType.Data);
            }
            else
            {
                Debug.Assert((entry.NodeFlags & NodeFlagsEnum.IsInline) ==
                             NodeFlagsEnum.IsInline);
                Collection.SetRawValue(optionalField.Name, entry.InlineData);
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
        // Part 1: write base fields (layout)
        if (Collection.Class.HasBaseFields)
        {
            var maxAlignment = Collection.Class.BaseFields.Max(f => f.Alignment);
            bw.AlignWriter(maxAlignment);
            
            DestinationLayoutPointer = bw.BaseStream.Position;

            foreach (var baseField in Collection.Class.BaseFields)
            {
                var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);

                bw.BaseStream.Position = DestinationLayoutPointer + baseField.Offset;

                var rawValue = Collection.GetRawValue(baseField.Name);
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
            if (entry.InlineData is not VltAttribType attrib)
            {
                continue;
            }

            var field = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext(Collection.Class, field, Collection);

            attrib.WritePointerData(context, fieldContext, bw);
        }

        // Part 3: Write pointer data for all fields
        foreach (var entry in Collection.GetOrderedData())
        {
            var field = Collection.Class[entry.Key];
            var fieldContext = new FieldReadWriteContext(Collection.Class, field, Collection);
            if (entry.Value is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.WritePointerData(context, fieldContext, bw);
            }
        }
    }

    public override void AddPointers(VaultWriteContext context)
    {
        context.AddPointer(SourceLayoutPointer, DestinationLayoutPointer, true);

        foreach (var baseField in Collection.Class.BaseFields)
        {
            var fieldContext = new FieldReadWriteContext(Collection.Class, baseField, Collection);
            var rawValue = Collection.GetRawValue(baseField.Name);

            if (rawValue is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }

        foreach (var entry in Entries)
        {
            var fieldContext = new FieldReadWriteContext(Collection.Class, Collection.Class[entry.Key], Collection);
            if (entry.InlineData is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.AddPointers(context, fieldContext);
            }
        }
    }

    private static long GetExpectedDataSize(VltClassField field, object value, long offset)
    {
        if (!field.IsArray)
        {
            return field.Size;
        }

        var array = (VltArrayType)value;
        var dataStartPos = offset + 8;
        var alignmentOffset = field.Alignment - 1;
        var alignedDataStartPos = (dataStartPos + alignmentOffset) & ~alignmentOffset;

        return (alignedDataStartPos - offset) + field.Size * array.Capacity;
    }
}