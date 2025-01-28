using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("AtttributeRefSpec")]
public abstract class AttributeRefSpec<TKey> : VltBaseType<TKey> where TKey : IKey<TKey>
{
    public TKey ClassKey { get; set; }
    public TKey DefinitionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        ClassKey = ReadKey(context, fieldContext, br);
        DefinitionKey = ReadKey(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        WriteKey(context, fieldContext, bw, ClassKey);
        WriteKey(context, fieldContext, bw, DefinitionKey);
    }

    protected abstract TKey ReadKey(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br);

    protected abstract void WriteKey(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw, TKey key);
}

public class AttributeRefSpec32 : AttributeRefSpec<Key32>
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

public class AttributeRefSpec64 : AttributeRefSpec<Key64>
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