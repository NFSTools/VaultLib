// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:15 PM.

using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types.Abstractions;
using VLT32Hasher = VaultLib.Core.Utils.VLT32Hasher;
using VLT64Hasher = VaultLib.Core.Utils.VLT64Hasher;

namespace VaultLib.Core.Types.Attrib
{
    [VLTTypeInfo("Attrib::RefSpec")]
    public class RefSpec : BaseRefSpec
    {
        public RefSpec(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RefSpec(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public override string ClassKey { get; set; }
        public override string CollectionKey { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            if (vault.Database.Options.Type == DatabaseType.X64Database)
            {
                // 64-bit RefSpec is 24 bytes instead of 12
                ClassKey = HashManager.ResolveVLT(br.ReadUInt64());
                CollectionKey = HashManager.ResolveVLT(br.ReadUInt64());
                br.ReadUInt64();
            }
            else
            {
                ClassKey = HashManager.ResolveVLT(br.ReadUInt32());
                CollectionKey = HashManager.ResolveVLT(br.ReadUInt32());
                br.ReadUInt32();
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (vault.Database.Options.Type == DatabaseType.X64Database)
            {
                bw.Write(VLT64Hasher.Hash(ClassKey));
                bw.Write(VLT64Hasher.Hash(CollectionKey));
                bw.Write(0L);
            }
            else
            {
                bw.Write(VLT32Hasher.Hash(ClassKey));
                bw.Write(VLT32Hasher.Hash(CollectionKey));
                bw.Write(0);
            }
        }
    }
}