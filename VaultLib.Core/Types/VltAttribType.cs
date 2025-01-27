// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 8:32 PM.

using System;
using CoreLibraries.IO;
using System.Diagnostics;
using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public class VltAttribType<TKey> : VltBaseType<TKey>, IVltPointerObject<TKey>
{
    private long _offsetDst;

    private long _offsetSrc;

    public uint Offset { get; set; } // pointer to bin stream
    public object Data { get; set; }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        Debug.Assert(Offset != 0);
        Debug.Assert(Offset % fieldContext.Field.Alignment == 0);

        br.BaseStream.Position = Offset;
        Data = context.Database.TypeRegistry.ReadFieldValue(context, fieldContext, br);

        if (Data is not VltArrayType<TKey>)
            Debug.Assert(br.BaseStream.Position - Offset == fieldContext.Field.Size,
                "br.BaseStream.Position - Offset == fieldContext.Field.Size");
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryWriter bw)
    {
        var field = fieldContext.Field;
        var minAlignment = field.IsArray ? 2 : 1;
        var actualAlignment = Math.Max(field.Alignment, minAlignment);

        bw.AlignWriter(actualAlignment);

        _offsetDst = bw.BaseStream.Position;
        context.Database.TypeRegistry.WriteFieldValue(Data, context, fieldContext, bw);
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

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryWriter bw)
    {
        _offsetSrc = bw.BaseStream.Position;
        bw.Write(0);
    }
}