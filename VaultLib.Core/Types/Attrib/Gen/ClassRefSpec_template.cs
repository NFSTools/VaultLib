// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:43 PM.

using System.IO;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib.Gen
{
    public abstract class ClassRefSpec_template : BaseRefSpec
    {
        protected ClassRefSpec_template(string classKey)
        {
            ClassKey = classKey;
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

                return _hash32 != 0
                    ? HashManager.ResolveVlt(_hash32)
                    : _hash64 != 0 ? HashManager.ResolveVlt(_hash64) : string.Empty;
            }
            set => _collectionKey = value;
        }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            if (context.Database.Options.Type == DatabaseType.X86Database)
            {
                _hash32 = br.ReadUInt32();
            }
            else
            {
                _hash64 = br.ReadUInt64();
            }
            br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            if (context.Database.Options.Type == DatabaseType.X86Database)
                bw.Write(Vlt32Hasher.Hash(CollectionKey));
            else
                bw.Write(Vlt64Hasher.Hash(CollectionKey));
            bw.Write(0);
        }

        public override string ToString()
        {
            return $"{ClassKey} -> {CollectionKey}";
        }

        // https://github.com/NFSTools/VaultLib/issues/13
        private uint _hash32;
        private ulong _hash64;
        private string _collectionKey;
    }
}