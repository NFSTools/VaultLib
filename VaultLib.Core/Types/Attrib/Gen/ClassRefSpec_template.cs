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
        CollectionKey = TKey.Read(br);

        br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        CollectionKey.Write(bw);
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
}

public abstract class ClassRefSpec_template64 : ClassRefSpec_template<Key64>
{
    protected ClassRefSpec_template64(string className) : base(className)
    {
    }

    protected ClassRefSpec_template64(Key64 classKey) : base(classKey)
    {
    }
}