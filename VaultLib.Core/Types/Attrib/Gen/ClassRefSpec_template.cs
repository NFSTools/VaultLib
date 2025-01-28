// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:43 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib.Gen;

public abstract class ClassRefSpec_template<TKey> : BaseRefSpec<TKey> where TKey : IKey<TKey>
{
    protected ClassRefSpec_template(string className)
    {
        ClassKey = TKey.FromString(className);
    }
    
    protected ClassRefSpec_template(TKey classKey)
    {
        ClassKey = classKey;
    }

    public sealed override TKey ClassKey { get; set; }

    public sealed override TKey CollectionKey
    {
        get;
        set;
    }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        CollectionKey = ReadKey(context, fieldContext, br);

        br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        WriteKey(context, fieldContext, bw, CollectionKey);
        bw.Write(0);
    }

    public override string ToString()
    {
        return $"{ClassKey} -> {CollectionKey}";
    }
}

public abstract class ClassRefSpec_template32 : ClassRefSpec_template<Key32>
{
    protected ClassRefSpec_template32(string className) : base(className)
    {
    }

    protected ClassRefSpec_template32(Key32 classKey) : base(classKey)
    {
    }

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

public abstract class ClassRefSpec_template64 : ClassRefSpec_template<Key64>
{
    protected ClassRefSpec_template64(string className) : base(className)
    {
    }

    protected ClassRefSpec_template64(Key64 classKey) : base(classKey)
    {
    }

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