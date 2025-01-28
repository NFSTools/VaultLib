// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/04/2019 @ 7:28 PM.

using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("GCollectionKey")]
public abstract class GCollectionKey<TKey> : BaseRefSpec<TKey> where TKey : IKey<TKey>
{
    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        CollectionKey = ReadKey(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        WriteKey(context, fieldContext, bw, CollectionKey);
    }

    public override TKey ClassKey
    {
        get => TKey.FromString("gameplay");
        set => throw new NotImplementedException("Setting ClassKey on a GCollectionKey is not allowed.");
    }

    public override TKey CollectionKey
    {
        get;
        set;
    }

    public override string ToString()
    {
        return $"gameplay -> {CollectionKey}";
    }
}