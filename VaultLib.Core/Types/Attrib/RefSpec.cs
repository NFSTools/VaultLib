// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:15 PM.

using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib
{
    [VLTTypeInfo("Attrib::RefSpec")]
    public class RefSpec : BaseRefSpec
    {
        public override string ClassKey { get; set; }
        public override string CollectionKey { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            if (vault.Database.Is64Bit)
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
            if (vault.Database.Is64Bit)
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

        public RefSpec(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public RefSpec(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}