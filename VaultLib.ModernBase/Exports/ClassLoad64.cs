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

public class ClassLoad64 : BaseClassLoad<Key64>
{
    private ulong ClassHash { get; set; }
    private int NumDefinitions { get; set; }

    private uint _definitionsPtr;
    private uint _staticDataPtr;

    private long _srcDefinitionsPtr;
    private long _srcStaticPtr;
    private long _dstDefinitionsPtr;
    private long _dstStaticPtr;

    public override Key64 GetExportId()
    {
        return Class.Key;
    }

    public override void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        ClassHash = br.ReadUInt64();
        br.ReadInt32();
        NumDefinitions = br.ReadInt32();
        _definitionsPtr = br.ReadPointer();
        var staticSize = br.ReadUInt32(); // static size
        _staticDataPtr = br.ReadPointer();
        var layoutSize = br.ReadUInt32(); // Total size of required fields
        br.ReadUInt16(); // can be 0
        br.ReadUInt16(); // Number of required fields
        br.ReadUInt32(); // align

        if (_definitionsPtr == 0)
        {
            throw new InvalidDataException("Definitions pointer is NULL, this is not good!");
        }

        Class = new VltClass<Key64>(new Key64(ClassHash))
        {
            LayoutSize = layoutSize,
            StaticSize = staticSize,
        };
    }

    public override void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
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
        bw.Write(0); // align
    }

    public override void ReadPointerData(VaultReadContext<Key64> context, BinaryReader br)
    {
        br.BaseStream.Position = _definitionsPtr;

        for (int i = 0; i < NumDefinitions; i++)
        {
            AttribDefinition64 definition = new AttribDefinition64();
            definition.Read(context, br);

            var field = new VltClassField<Key64>(
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

                var fieldContext = new FieldReadWriteContext<Key64>(Class, staticField, null);
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
            var fieldContext = new FieldReadWriteContext<Key64>(Class, staticField, null);
            if (staticField.StaticValue is IVltPointerObject<Key64> vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }

        context.Database.AddClass(Class);
    }

    public override void WritePointerData(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.AlignWriter(8);
        _dstDefinitionsPtr = bw.BaseStream.Position;

        foreach (var (_, field) in Class.Fields.OrderBy(f => f.Key))
        {
            var definition = new AttribDefinition64
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
                var fieldContext = new FieldReadWriteContext<Key64>(Class, staticField, null);
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
                var fieldContext = new FieldReadWriteContext<Key64>(Class, staticField, null);
                if (staticField.StaticValue is IVltPointerObject<Key64> vltPointerObject)
                {
                    vltPointerObject.WritePointerData(context, fieldContext, bw);
                }
            }
        }
    }

    public override void AddPointers(VaultWriteContext<Key64> context)
    {
        context.AddPointer(_srcDefinitionsPtr, _dstDefinitionsPtr, true);

        if (_srcStaticPtr != 0 && _dstStaticPtr != 0)
        {
            context.AddPointer(_srcStaticPtr, _dstStaticPtr, true);

            foreach (var staticField in Class.StaticFields)
            {
                var fieldContext = new FieldReadWriteContext<Key64>(Class, staticField, null);
                if (staticField.StaticValue is IVltPointerObject<Key64> vltPointerObject)
                {
                    vltPointerObject.AddPointers(context, fieldContext);
                }
            }
        }
    }
}