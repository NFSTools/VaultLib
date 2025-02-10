using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(PhysicsUpgrade))]
public class PhysicsUpgrade : VltBaseType<Core.DataInterfaces.Key32>
{
    public AttributeRefSpec32 ReferencedAttribute { get; set; } = new();
    public float BlendingPower { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        ReferencedAttribute.Read(context, fieldContext, br);
        BlendingPower = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        ReferencedAttribute.Write(context, fieldContext, bw);
        bw.Write(BlendingPower);
    }

    public override object Clone()
    {
        return new PhysicsUpgrade
        {
            ReferencedAttribute = (AttributeRefSpec32)ReferencedAttribute.Clone(),
            BlendingPower = BlendingPower,
        };
    }
}