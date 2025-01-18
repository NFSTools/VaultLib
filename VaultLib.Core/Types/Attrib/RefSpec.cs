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
    [VltTypeInfo("Attrib::RefSpec")]
    public class RefSpec : BaseRefSpec
    {
        public RefSpec(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RefSpec(VltClass @class, VltClassField field) : base(@class, field)
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
                    ? HashManager.ResolveVlt(_collectionHash32)
                    : _collectionHash64 != 0 ? HashManager.ResolveVlt(_collectionHash64) : string.Empty;
            }
            set => _collectionKey = value;
        }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            if (context.Database.Options.Type == DatabaseType.X64Database)
            {
                // 64-bit RefSpec is 24 bytes instead of 12
                ClassKey = HashManager.ResolveVlt(br.ReadUInt64());
                _collectionHash64 = br.ReadUInt64();
                br.ReadUInt64();
            }
            else
            {
                ClassKey = HashManager.ResolveVlt(br.ReadUInt32());
                _collectionHash32 = br.ReadUInt32();
                br.ReadUInt32();
            }
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            if (context.Database.Options.Type == DatabaseType.X64Database)
            {
                bw.Write(Vlt64Hasher.Hash(ClassKey));
                bw.Write(Vlt64Hasher.Hash(CollectionKey));
                bw.Write(0L);
            }
            else
            {
                bw.Write(Vlt32Hasher.Hash(ClassKey));
                bw.Write(Vlt32Hasher.Hash(CollectionKey));
                bw.Write(0);
            }
        }

        // https://github.com/NFSTools/VaultLib/issues/13
        private uint _collectionHash32;
        private ulong _collectionHash64;
        private string _collectionKey;
    }
}