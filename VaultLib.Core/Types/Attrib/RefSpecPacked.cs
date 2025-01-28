// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:15 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib;

public abstract class RefSpecPacked<TKey> : BaseRefSpec<TKey> where TKey : struct, IKey<TKey>
{
    public override TKey ClassKey { get; set; }

    public override TKey CollectionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        ClassKey = TKey.Read(br);
        CollectionKey = TKey.Read(br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        ClassKey.Write(bw);
        CollectionKey.Write(bw);
    }
}

public class RefSpecPacked32 : RefSpecPacked<Key32>
{
}

public class RefSpecPacked64 : RefSpecPacked<Key64>
{
}