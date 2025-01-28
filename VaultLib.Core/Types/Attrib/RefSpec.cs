// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:15 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib;

[VltTypeInfo("Attrib::RefSpec")]
public abstract class RefSpec<TKey> : BaseRefSpec<TKey> where TKey : IKey<TKey>
{
    public override TKey ClassKey { get; set; }

    public override TKey CollectionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        ClassKey = ReadKey(context, fieldContext, br);
        CollectionKey = ReadKey(context, fieldContext, br);
        ReadKey(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        WriteKey(context, fieldContext, bw, ClassKey);
        WriteKey(context, fieldContext, bw, CollectionKey);
        WriteKey(context, fieldContext, bw, TKey.Zero);
    }
}

public class RefSpec32 : RefSpec<Key32>
{
    protected override Key32 ReadKey(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        return new Key32(br.ReadUInt32());
    }

    protected override void WriteKey(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw, Key32 key)
    {
        bw.Write(key.Hash);
    }
}

public class RefSpec64 : RefSpec<Key64>
{
    protected override Key64 ReadKey(VaultReadContext<Key64> context, FieldReadWriteContext<Key64> fieldContext,
        BinaryReader br)
    {
        return new Key64(br.ReadUInt64());
    }

    protected override void WriteKey(VaultWriteContext<Key64> context, FieldReadWriteContext<Key64> fieldContext,
        BinaryWriter bw, Key64 key)
    {
        bw.Write(key.Hash);
    }
}