// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/04/2019 @ 7:28 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(GCollectionKey))]
    public class GCollectionKey : BaseRefSpec
    {
        public override void Read(Vault vault, BinaryReader br)
        {
            if (vault.Database.Options.Type == DatabaseType.X86Database)
            {
                _hash32 = br.ReadUInt32();
            }
            else
            {
                _hash64 = br.ReadUInt64();
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (vault.Database.Options.Type == DatabaseType.X86Database)
                bw.Write(VLT32Hasher.Hash(_key));
            else
                bw.Write(VLT64Hasher.Hash(_key));
        }

        public override string ClassKey
        {
            get => "gameplay";
            set { }
        }

        public override string CollectionKey
        {
            get => _key = _hash32 != 0 ? HashManager.ResolveVLT(_hash32) : HashManager.ResolveVLT(_hash64);
            set => _key = value;
        }
        public override bool CanChangeClass => false;

        public override string ToString()
        {
            return $"gameplay -> {CollectionKey}";
        }

        public GCollectionKey(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GCollectionKey(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        // https://github.com/NFSTools/VaultLib/issues/13
        private uint _hash32;
        private ulong _hash64;
        private string _key;
    }
}