using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(type_bStringHash))]
    public class type_bStringHash : VLTBaseType
    {
        public uint Hash { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Hash = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Hash);
        }

        public type_bStringHash(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public type_bStringHash(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}