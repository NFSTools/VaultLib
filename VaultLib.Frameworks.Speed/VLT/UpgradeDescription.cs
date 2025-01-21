using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(UpgradeDescription))]
public class UpgradeDescription : VltBaseType
{
    public RefSpec mPhysicsUpgradeSet { get; set; } = new();
    public float mBlendingPower { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        mPhysicsUpgradeSet.Read(context, fieldContext, br);
        mBlendingPower = br.ReadSingle();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        mPhysicsUpgradeSet.Write(context, fieldContext, bw);
        bw.Write(mBlendingPower);
    }
}