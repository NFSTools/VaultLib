// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:15 PM.

using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib
{
    public class RefSpecPacked : BaseRefSpec
    {
        public RefSpecPacked(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RefSpecPacked(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public override string ClassKey { get; set; }

        public override string CollectionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_collectionKey))
                {
                    return _collectionKey;
                }

                return _collectionHash32 != 0
                    ? HashManager.ResolveVLT(_collectionHash32)
                    : _collectionHash64 != 0 ? HashManager.ResolveVLT(_collectionHash64) : string.Empty;
            }
            set => _collectionKey = value;
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            if (vault.Database.Options.Type == DatabaseType.X64Database)
            {
                // 64-bit RefSpec is 16 bytes instead of 8
                ClassKey = HashManager.ResolveVLT(br.ReadUInt64());
                _collectionHash64 = br.ReadUInt64();
            }
            else
            {
                ClassKey = HashManager.ResolveVLT(br.ReadUInt32());
                _collectionHash32 = br.ReadUInt32();
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (vault.Database.Options.Type == DatabaseType.X64Database)
            {
                bw.Write(VLT64Hasher.Hash(ClassKey));
                bw.Write(VLT64Hasher.Hash(CollectionKey));
            }
            else
            {
                bw.Write(VLT32Hasher.Hash(ClassKey));
                bw.Write(VLT32Hasher.Hash(CollectionKey));
            }
        }

        // https://github.com/NFSTools/VaultLib/issues/13
        private uint _collectionHash32;
        private ulong _collectionHash64;
        private string _collectionKey;
    }
}