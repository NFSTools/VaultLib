using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::EnhancerPart")]
    public class EnhancerPart : VLTBaseType
    {
        public EnhancerPart(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public EnhancerPart(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint Hash { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Hash = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Hash);
        }
    }
}