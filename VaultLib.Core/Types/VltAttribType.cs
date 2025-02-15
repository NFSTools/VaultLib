// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 8:32 PM.

using System;
using CoreLibraries.IO;
using System.Diagnostics;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public class VltAttribType<TKey> : VltBaseType<TKey>, IVltPointerObject<TKey> where TKey : struct, IKey<TKey>
{
    private long _offsetDst;

    private long _offsetSrc;

    public uint Offset { get; set; } // pointer to bin stream
    public object Data { get; set; }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br)
    {
        Debug.Assert(Offset != 0);
        Debug.Assert(Offset % fieldContext.Field.Alignment == 0);

        br.BaseStream.Position = Offset;
        Data = context.Database.TypeRegistry.ReadFieldValue(context, fieldContext, br);

        if (Data is not VltArrayType<TKey>)
        {
            var bytesRead = br.BaseStream.Position - Offset;
            var type = context.Database.TypeRegistry.ResolveFieldType(fieldContext.Field);
            Debug.Assert(bytesRead == fieldContext.Field.Size,
                "bytesRead == fieldContext.Field.Size",
                $"Read {bytesRead} bytes for attribute of type {type} instead of expected {fieldContext.Field.Size}");
        }
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        var field = fieldContext.Field;
        var minAlignment = field.IsArray ? 2 : 1;
        var actualAlignment = Math.Max(field.Alignment, minAlignment);

        bw.AlignWriter(actualAlignment);

        _offsetDst = bw.BaseStream.Position;
        
        var startPosition = bw.BaseStream.Position;
        context.Database.TypeRegistry.WriteFieldValue(Data, context, fieldContext, bw);

        if (Data is not VltArrayType<TKey>)
        {
            var bytesWritten = bw.BaseStream.Position - startPosition;
            var type = context.Database.TypeRegistry.ResolveFieldType(fieldContext.Field);
            Debug.Assert(bytesWritten == fieldContext.Field.Size,
                "bytesWritten == fieldContext.Field.Size",
                $"Wrote {bytesWritten} bytes for attribute of type {type} instead of expected {fieldContext.Field.Size}");
        }
    }

    public void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext)
    {
        Debug.Assert(_offsetSrc != 0 && _offsetDst != 0);

        context.AddPointer(_offsetSrc, _offsetDst, true);

        if (Data is IVltPointerObject<TKey> vltPointerObject)
        {
            vltPointerObject.AddPointers(context, fieldContext);
        }
    }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        Offset = br.ReadPointer();
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _offsetSrc = bw.BaseStream.Position;
        bw.Write(0);
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}