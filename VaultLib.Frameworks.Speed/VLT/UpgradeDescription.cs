using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(UpgradeDescription))]
public class UpgradeDescription: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public RefSpec32 mPhysicsUpgradeSet { get; set; } = new();
    public float mBlendingPower { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        mPhysicsUpgradeSet.Read(context, fieldContext, br);
        mBlendingPower = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        mPhysicsUpgradeSet.Write(context, fieldContext, bw);
        bw.Write(mBlendingPower);
    }
}