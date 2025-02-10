// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:49 PM.

using System;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public class VltListContainer<TKey, TItem> : VltBaseType<TKey>, IVltPointerObject<TKey> where TKey : struct, IKey<TKey>
{
    private long _dstPtr;

    private uint _pointer;

    private long _srcPtr;

    public VltListContainer(int count) : this(new List<TItem>(count))
    {
    }

    public VltListContainer(List<TItem> items)
    {
        Items = items;
    }

    public List<TItem> Items { get; }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br)
    {
        br.BaseStream.Position = _pointer;

        var databaseTypeRegistry = context.Database.TypeRegistry;
        for (var i = 0; i < Items.Capacity; i++)
        {
            // var item = (T)databaseTypeRegistry.ConstructTypeInstance(typeof(T));
            //var item = (T) Activator.CreateInstance(typeof(T), Class, Field, Collection);
            var item = (TItem)databaseTypeRegistry.ReadTypeInstance(context, fieldContext, br, typeof(TItem));
            Items.Add(item);
        }
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _dstPtr = bw.BaseStream.Position;

        foreach (var item in Items)
        {
            // item.Write(context, fieldContext, bw);
            context.Database.TypeRegistry.WriteTypeInstance(item, context, fieldContext, bw, typeof(TItem));
        }
    }

    public void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext)
    {
        context.AddPointer(_srcPtr, _dstPtr, false);
    }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        _pointer = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _srcPtr = bw.BaseStream.Position;
        bw.Write(0);
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}