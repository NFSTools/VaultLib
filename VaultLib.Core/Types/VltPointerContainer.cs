// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:56 PM.

using System;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

/// <summary>
///     Helper class for reading data types through a pointer
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TItem"></typeparam>
public class VltPointerContainer<TKey, TItem> : VltBaseType<TKey>, IVltPointerObject<TKey>
    where TKey : struct, IKey<TKey>
{
    private uint _pointer;
    private long _ptrDst;

    private long _ptrSrc;

    public TItem Value { get; set; }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br)
    {
        br.BaseStream.Position = _pointer;
        Value = (TItem)context.Database.TypeRegistry.ReadTypeInstance(context, fieldContext, br, typeof(TItem));

        if (Value is IVltPointerObject<TKey> vltPointerObject)
        {
            vltPointerObject.ReadPointerData(context, fieldContext, br);
        }
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _ptrDst = bw.BaseStream.Position;
        context.Database.TypeRegistry.WriteTypeInstance(Value, context, fieldContext, bw, typeof(TItem));

        if (Value is IVltPointerObject<TKey> vltPointerObject)
        {
            vltPointerObject.WritePointerData(context, fieldContext, bw);
        }
    }

    public void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext)
    {
        context.AddPointer(_ptrSrc, _ptrDst, false);
    }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        _pointer = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _ptrSrc = bw.BaseStream.Position;
        bw.Write(0);
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}