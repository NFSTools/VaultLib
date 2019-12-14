// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:43 PM.

using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types.Abstractions;

namespace VaultLib.Core.Types.Attrib.Gen
{
    public abstract class ClassRefSpec_template : BaseRefSpec
    {
        protected ClassRefSpec_template(VltClass @class, VltClassField field, VltCollection collection, string classKey)
            : base(@class, field, collection)
        {
            ClassKey = classKey;
        }

        protected ClassRefSpec_template(VltClass @class, VltClassField field, string classKey) : base(@class, field)
        {
            ClassKey = classKey;
        }

        public override bool CanChangeClass => false;

        public override string ClassKey { get; set; }

        public override string CollectionKey { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            CollectionKey = HashManager.ResolveVLT(br.ReadUInt32());
            br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(VLT32Hasher.Hash(CollectionKey));
            bw.Write(0);
        }

        public override string ToString()
        {
            return $"{ClassKey} -> {CollectionKey}";
        }
    }
}