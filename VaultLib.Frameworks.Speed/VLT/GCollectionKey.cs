// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/04/2019 @ 7:28 PM.

using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("GCollectionKey")]
public abstract class GCollectionKey<TKey> : BaseRefSpec<TKey> where TKey : struct, IKey<TKey>
{
    public TKey CollectionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        CollectionKey = TKey.Read(br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        CollectionKey.Write(bw);
    }

    public override TKey GetClassKey()
    {
        return TKey.FromString("gameplay");
    }

    public override TKey GetCollectionKey()
    {
        return CollectionKey;
    }

    public override void SetClassKey(TKey classKey)
    {
        throw new NotImplementedException("Setting ClassKey on a GCollectionKey is not allowed.");
    }

    public override void SetCollectionKey(TKey collectionKey)
    {
        CollectionKey = collectionKey;
    }

    public override string ToString()
    {
        return $"gameplay -> {CollectionKey}";
    }
}

public class GCollectionKey32 : GCollectionKey<Key32>
{
    public override object Clone()
    {
        return new GCollectionKey32
        {
            CollectionKey = this.CollectionKey,
        };
    }
}

public class GCollectionKey64 : GCollectionKey<Key64>
{
    public override object Clone()
    {
        return new GCollectionKey64
        {
            CollectionKey = this.CollectionKey,
        };
    }
}