using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo("AtttributeRefSpec")]
    public class AttributeRefSpec : VLTBaseType
    {
        public AttributeRefSpec(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public AttributeRefSpec(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public string ClassKey { get; set; }
        public string DefinitionKey { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            if (vault.Database.Options.Type == DatabaseType.X86Database)
            {
                ClassKey = HashManager.ResolveVLT(br.ReadUInt32());
                DefinitionKey = HashManager.ResolveVLT(br.ReadUInt32());
            }
            else
            {
                ClassKey = HashManager.ResolveVLT(br.ReadUInt64());
                DefinitionKey = HashManager.ResolveVLT(br.ReadUInt64());
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (vault.Database.Options.Type == DatabaseType.X86Database)
            {
                bw.Write(VLT32Hasher.Hash(ClassKey));
                bw.Write(VLT32Hasher.Hash(DefinitionKey));
            }
            else
            {
                bw.Write(VLT64Hasher.Hash(ClassKey));
                bw.Write(VLT64Hasher.Hash(DefinitionKey));
            }
        }
    }
}