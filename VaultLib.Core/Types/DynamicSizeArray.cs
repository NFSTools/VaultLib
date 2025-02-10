// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:40 PM.

using System.IO;
using System.Linq;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public class DynamicSizeArray<TKey, TItem> : VltBaseType<TKey>, IVltPointerObject<TKey>
    where TItem : VltBaseType<TKey> where TKey : struct, IKey<TKey>
{
    private long _dstPtr;

    private uint _pointer;
    private long _srcPtr;

    public TItem[] Items { get; set; }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br)
    {
        var databaseTypeRegistry = context.Database.TypeRegistry;

        br.BaseStream.Position = _pointer;
        for (var i = 0; i < Items.Length; i++)
        {
            Items[i] = (TItem)databaseTypeRegistry.ConstructTypeInstance(typeof(TItem));
            Items[i].Read(context, fieldContext, br);
        }
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _dstPtr = bw.BaseStream.Position;
        foreach (var vltBaseType in Items) vltBaseType.Write(context, fieldContext, bw);
    }

    public void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext)
    {
        context.AddPointer(_srcPtr, _dstPtr, false);
    }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        _pointer = br.ReadUInt32();
        Items = new TItem[br.ReadInt32()];
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        _srcPtr = bw.BaseStream.Position;
        bw.Write(0);
        bw.Write(Items.Length);
    }

    public override object Clone()
    {
        return new DynamicSizeArray<TKey, TItem>
        {
            Items = this.Items.Select(i => (TItem)i.Clone()).ToArray(),
        };
    }
}