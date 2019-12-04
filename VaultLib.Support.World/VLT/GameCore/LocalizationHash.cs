using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::LocalizationHash")]
    public class LocalizationHash : VLTBaseType
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

        public LocalizationHash(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public LocalizationHash(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}