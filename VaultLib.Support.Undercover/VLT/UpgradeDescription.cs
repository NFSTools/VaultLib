using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(UpgradeDescription))]
    public class UpgradeDescription : VLTBaseType
    {
        public UpgradeDescription(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public UpgradeDescription(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RefSpec mPhysicsUpgradeSet { get; set; }
        public float mBlendingPower { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mPhysicsUpgradeSet = new RefSpec(Class, Field, Collection);
            mPhysicsUpgradeSet.Read(vault, br);
            mBlendingPower = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            mPhysicsUpgradeSet.Write(vault, bw);
            bw.Write(mBlendingPower);
        }
    }
}