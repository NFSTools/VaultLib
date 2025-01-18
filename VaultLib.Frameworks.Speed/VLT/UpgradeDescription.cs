using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(UpgradeDescription))]
    public class UpgradeDescription : VltBaseType
    {
        public UpgradeDescription(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            mPhysicsUpgradeSet = new RefSpec(Class, Field, Collection);
        }

        public RefSpec mPhysicsUpgradeSet { get; set; }
        public float mBlendingPower { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            mPhysicsUpgradeSet.Read(context, br);
            mBlendingPower = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            mPhysicsUpgradeSet.Write(context, bw);
            bw.Write(mBlendingPower);
        }
    }
}