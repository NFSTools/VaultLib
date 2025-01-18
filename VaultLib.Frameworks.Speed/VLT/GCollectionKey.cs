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

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(GCollectionKey))]
    public class GCollectionKey : BaseRefSpec
    {
        public override void Read(VaultReadContext context, BinaryReader br)
        {
            if (context.Database.Options.Type == DatabaseType.X86Database)
            {
                _hash32 = br.ReadUInt32();
            }
            else
            {
                _hash64 = br.ReadUInt64();
            }
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            if (context.Database.Options.Type == DatabaseType.X86Database)
                bw.Write(Vlt32Hasher.Hash(CollectionKey));
            else
                bw.Write(Vlt64Hasher.Hash(CollectionKey));
        }

        public override string ClassKey
        {
            get => "gameplay";
            set { }
        }

        public override string CollectionKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    return _key;
                }

                return _hash32 != 0
                    ? HashManager.ResolveVlt(_hash32)
                    : _hash64 != 0 ? HashManager.ResolveVlt(_hash64) : string.Empty;
            }
            set => _key = value;
        }

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