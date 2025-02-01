using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Utils;

namespace VaultLib.ModernBase.Exports;

public class ClassLoad32 : BaseClassLoad<Key32>
{
    private uint ClassHash { get; set; }
    private int NumDefinitions { get; set; }

    private uint _definitionsPtr;
    private uint _staticDataPtr;

    private long _srcDefinitionsPtr;
    private long _srcStaticPtr;
    private long _dstDefinitionsPtr;
    private long _dstStaticPtr;

    public override void Read(VaultReadContext<Key32> context, BinaryReader br)
    {
        ClassHash = br.ReadUInt32();
        br.ReadUInt32(); // Collection reserve
        int mNumDefinitions = br.ReadInt32(); // Number of fields
        _definitionsPtr = br.ReadPointer();
        uint staticSize = br.ReadUInt32(); // static size
        _staticDataPtr = br.ReadPointer();
        var layoutSize = br.ReadUInt32(); // Total size of required fields
        br.ReadUInt16(); // can be 0
        br.ReadUInt16(); // Number of required fields

        if (_definitionsPtr == 0)
        {
            throw new InvalidDataException("Definitions pointer is NULL, this is not good!");
        }

        NumDefinitions = mNumDefinitions;
        Class = new VltClass<Key32>(new Key32(ClassHash))
        {
            LayoutSize = layoutSize,
            StaticSize = staticSize,
        };

        // Debug.WriteLine("class load: {0} - layout size = {1}, static size = {2}", Class.Name, layoutSize, staticSize);
    }

    public override void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        int collectionReserve = (from collection in context.Collections
            where collection.Class.Key == Class.Key
            select collection).Count();

        bw.Write(Class.Key.Hash);
        bw.Write(collectionReserve);
        bw.Write(Class.Fields.Count);
        _srcDefinitionsPtr = bw.BaseStream.Position;
        bw.Write(0);
        bw.Write(Class.StaticSize);

        if (Class.StaticSize > 0)
        {
            _srcStaticPtr = bw.BaseStream.Position;
        }

        bw.Write(0);

        bw.Write(Class.LayoutSize);
        bw.Write((ushort)0);
        bw.Write((ushort)Class.BaseFields.Count());
    }

    public override void ReadPointerData(VaultReadContext<Key32> context, BinaryReader br)
    {
        br.BaseStream.Position = _definitionsPtr;

        for (int i = 0; i < NumDefinitions; i++)
        {
            AttribDefinition32 definition = new AttribDefinition32();
            definition.Read(context, br);

            var field = new VltClassField<Key32>(
                Class,
                definition.Key,
                definition.Type,
                definition.Flags,
                definition.Alignment,
                definition.Size,
                definition.MaxCount,
                definition.Offset);

            Class.Fields.Add(definition.Key, field);
        }

        if (_staticDataPtr != 0)
        {
            br.BaseStream.Position = _staticDataPtr;

            foreach (var staticField in Class.StaticFields)
            {
                br.SafeAlignReader(staticField.Alignment);

                var fieldContext = new FieldReadWriteContext<Key32>(Class, staticField, null);
                var staticData =
                    context.Database.TypeRegistry.ReadFieldValue(context, fieldContext,
                        br);
                staticField.StaticValue = staticData;
            }

            var staticEndPos = br.BaseStream.Position;

            if (staticEndPos - _staticDataPtr > Class.StaticSize)
            {
                throw new Exception("read too much static data, something went wrong!");
            }
        }

        foreach (var staticField in Class.StaticFields)
        {
            var fieldContext = new FieldReadWriteContext<Key32>(Class, staticField, null);
            if (staticField.StaticValue is IVltPointerObject<Key32> vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }

        context.Database.AddClass(Class);
    }

    public override void WritePointerData(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        bw.AlignWriter(0x8);
        _dstDefinitionsPtr = bw.BaseStream.Position;

        foreach (var (_, field) in Class.Fields.OrderBy(f => f.Key))
        {
            var definition = new AttribDefinition32
            {
                Key = field.Key,
                Alignment = field.Alignment,
                Flags = field.Flags,
                MaxCount = field.MaxCount,
                Offset = field.Offset,
                Size = field.Size,
                Type = field.TypeKey
            };
            definition.Write(context, bw);
        }

        if (_srcStaticPtr != 0)
        {
            _dstStaticPtr = bw.BaseStream.Position;

            foreach (var staticField in Class.StaticFields)
            {
                bw.AlignWriter(staticField.Alignment);
                var fieldContext = new FieldReadWriteContext<Key32>(Class, staticField, null);
                context.Database.TypeRegistry.WriteFieldValue(staticField.StaticValue, context,
                    fieldContext, bw);
            }

            var staticEndPos = bw.BaseStream.Position;
            var actualStaticSize = (int)(staticEndPos - _dstStaticPtr);
            var configuredStaticSize = (int)Class.StaticSize;

            if (actualStaticSize > configuredStaticSize)
            {
                throw new Exception("wrote too much static data");
            }

            if (actualStaticSize < configuredStaticSize)
            {
                var remaining = configuredStaticSize - actualStaticSize;
                bw.Write(new byte[remaining]);
            }

            foreach (var staticField in Class.StaticFields)
            {
                var fieldContext = new FieldReadWriteContext<Key32>(Class, staticField, null);
                if (staticField.StaticValue is IVltPointerObject<Key32> vltPointerObject)
                {
                    vltPointerObject.WritePointerData(context, fieldContext, bw);
                }
            }
        }
    }

    public override void AddPointers(VaultWriteContext<Key32> context)
    {
        context.AddPointer(_srcDefinitionsPtr, _dstDefinitionsPtr, true);

        if (_srcStaticPtr != 0 && _dstStaticPtr != 0)
        {
            context.AddPointer(_srcStaticPtr, _dstStaticPtr, true);

            foreach (var staticField in Class.StaticFields)
            {
                var fieldContext = new FieldReadWriteContext<Key32>(Class, staticField, null);
                if (staticField.StaticValue is IVltPointerObject<Key32> vltPointerObject)
                {
                    vltPointerObject.AddPointers(context, fieldContext);
                }
            }
        }
    }

    public override Key32 GetExportId()
    {
        return Class.Key;
    }

    private int ComputeLayoutSize()
    {
        if (!Class.HasBaseFields)
            return 0;

        var layoutSize = 0;
        var packingRequirement = 1;
        foreach (var baseField in Class.BaseFields)
        {
            if (layoutSize % baseField.Alignment != 0)
            {
                layoutSize += baseField.Alignment - layoutSize % baseField.Alignment;
            }

            if ((baseField.Flags & DefinitionFlags.Array) != 0)
            {
                layoutSize += 8;
                layoutSize += baseField.Size * baseField.MaxCount;
            }
            else
            {
                layoutSize += baseField.Size;
            }

            packingRequirement = Math.Max(packingRequirement, baseField.Alignment);
        }

        Debug.Assert((packingRequirement & (packingRequirement - 1)) == 0);

        return (layoutSize + packingRequirement - 1) & ~(packingRequirement - 1);
    }

    private int ComputeStaticSize()
    {
        int staticSize = 0;

        foreach (var vltClassField in Class.StaticFields)
        {
            if (staticSize % vltClassField.Alignment != 0)
            {
                staticSize += vltClassField.Alignment - staticSize % vltClassField.Alignment;
            }

            staticSize += vltClassField.Size;
        }

        return staticSize;
    }
}