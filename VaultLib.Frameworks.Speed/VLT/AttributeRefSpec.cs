using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("AtttributeRefSpec")]
public class AttributeRefSpec : VltBaseType
{
    public string ClassKey { get; set; }
    public string DefinitionKey { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
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

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
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