// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:15 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Types.Attrib;

[VltTypeInfo("Attrib::RefSpec")]
public abstract class RefSpec<TKey> : BaseRefSpec<TKey> where TKey : struct, IKey<TKey>
{
    public TKey ClassKey { get; set; }

    public TKey CollectionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        ClassKey = TKey.Read(br);
        CollectionKey = TKey.Read(br);
        TKey.Read(br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        ClassKey.Write(bw);
        CollectionKey.Write(bw);
        TKey.Zero.Write(bw);
    }

    public override TKey GetClassKey()
    {
        return ClassKey;
    }

    public override TKey GetCollectionKey()
    {
        return CollectionKey;
    }

    public override void SetClassKey(TKey classKey)
    {
        ClassKey = classKey;
    }

    public override void SetCollectionKey(TKey collectionKey)
    {
        CollectionKey = collectionKey;
    }
}

public class RefSpec32 : RefSpec<Key32>
{
    public override object Clone()
    {
        return new RefSpec32
        {
            ClassKey = this.ClassKey,
            CollectionKey = this.CollectionKey,
        };
    }
}

public class RefSpec64 : RefSpec<Key64>
{
    public override object Clone()
    {
        return new RefSpec64
        {
            ClassKey = this.ClassKey,
            CollectionKey = this.CollectionKey,
        };
    }
}