using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(FEDefaultUnlockable))]
    public class FEDefaultUnlockable : VLTBaseType
    {
        public FEDefaultUnlockable(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEDefaultUnlockable(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eUnlockableEntity UnlockType { get; set; }
        public uint UnlockName { get; set; }
        public int UnlockLevel { get; set; }
        public int UnlockTier { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            UnlockType = br.ReadEnum<eUnlockableEntity>();
            UnlockName = br.ReadUInt32();
            UnlockLevel = br.ReadInt32();
            UnlockTier = br.ReadInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(UnlockType);
            bw.Write(UnlockName);
            bw.Write(UnlockLevel);
            bw.Write(UnlockTier);
        }
    }
}