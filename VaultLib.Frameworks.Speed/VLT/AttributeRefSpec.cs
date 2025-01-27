using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("AtttributeRefSpec")]
public abstract class AttributeRefSpec<TKey> : VltBaseType<TKey>
{
    public string ClassKey { get; set; }
    public string DefinitionKey { get; set; }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        if (context.Database.Options.Type == DatabaseType.X86Database)
        {
            ClassKey = HashManager.ResolveVlt(br.ReadUInt32());
            DefinitionKey = HashManager.ResolveVlt(br.ReadUInt32());
        }
        else
        {
            ClassKey = HashManager.ResolveVlt(br.ReadUInt64());
            DefinitionKey = HashManager.ResolveVlt(br.ReadUInt64());
        }
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryWriter bw)
    {
        if (context.Database.Options.Type == DatabaseType.X86Database)
        {
            bw.Write(Vlt32Hasher.Hash(ClassKey));
            bw.Write(Vlt32Hasher.Hash(DefinitionKey));
        }
        else
        {
            bw.Write(Vlt64Hasher.Hash(ClassKey));
            bw.Write(Vlt64Hasher.Hash(DefinitionKey));
        }
    }
}

public class AttributeRefSpec32 : AttributeRefSpec<uint> {}
public class AttributeRefSpec64 : AttributeRefSpec<ulong> {}