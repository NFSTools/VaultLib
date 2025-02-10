using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("AtttributeRefSpec")]
public abstract class AttributeRefSpec<TKey> : VltBaseType<TKey> where TKey : struct, IKey<TKey>
{
    public TKey ClassKey { get; set; }
    public TKey DefinitionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        ClassKey = TKey.Read(br);
        DefinitionKey = TKey.Read(br);
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        ClassKey.Write(bw);
        DefinitionKey.Write(bw);
    }
}

public class AttributeRefSpec32 : AttributeRefSpec<Key32>
{
    public override object Clone()
    {
        return new AttributeRefSpec32
        {
            ClassKey = this.ClassKey,
            DefinitionKey = this.DefinitionKey,
        };
    }
}

public class AttributeRefSpec64 : AttributeRefSpec<Key64>
{
    public override object Clone()
    {
        return new AttributeRefSpec64
        {
            ClassKey = this.ClassKey,
            DefinitionKey = this.DefinitionKey,
        };
    }
}