using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(PhysicsUpgrade))]
public class PhysicsUpgrade: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public AttributeRefSpec32 ReferencedAttribute { get; set; } = new();
    public float BlendingPower { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        ReferencedAttribute.Read(context, fieldContext, br);
        BlendingPower = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        ReferencedAttribute.Write(context, fieldContext, bw);
        bw.Write(BlendingPower);
    }
}