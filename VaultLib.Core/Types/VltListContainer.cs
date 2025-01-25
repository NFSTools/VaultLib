// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:49 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public class VltListContainer<T> : VltBaseType, IVltPointerObject
{
    private long _dstPtr;

    private uint _pointer;

    private long _srcPtr;

    public VltListContainer(int count) : this(new List<T>(count))
    {
    }

    public VltListContainer(List<T> items)
    {
        Items = items;
    }

    public List<T> Items { get; }

    public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        br.BaseStream.Position = _pointer;

        var databaseTypeRegistry = context.Database.TypeRegistry;
        for (var i = 0; i < Items.Capacity; i++)
        {
            // var item = (T)databaseTypeRegistry.ConstructTypeInstance(typeof(T));
            //var item = (T) Activator.CreateInstance(typeof(T), Class, Field, Collection);
            var item = (T)databaseTypeRegistry.ReadTypeInstance(context, fieldContext, br, typeof(T));
            Items.Add(item);
        }
    }

    public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        _dstPtr = bw.BaseStream.Position;

        foreach (var item in Items)
        {
            // item.Write(context, fieldContext, bw);
            context.Database.TypeRegistry.WriteTypeInstance(item, context, fieldContext, bw, typeof(T));
        }
    }

    public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
    {
        context.AddPointer(_srcPtr, _dstPtr, false);
    }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        _pointer = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        _srcPtr = bw.BaseStream.Position;
        bw.Write(0);
    }
}