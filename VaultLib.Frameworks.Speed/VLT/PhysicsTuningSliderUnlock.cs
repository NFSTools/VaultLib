using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PhysicsTuningSliderUnlock))]
public class PhysicsTuningSliderUnlock : VltBaseType<Core.DataInterfaces.Key32>
{
    public RefSpec32 PhysicsTuningSlider { get; set; } = new();
    public float Range { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PhysicsTuningSlider.Read(context, fieldContext, br);
        Range = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        PhysicsTuningSlider.Write(context, fieldContext, bw);
        bw.Write(Range);
    }

    public override object Clone()
    {
        return new PhysicsTuningSliderUnlock
        {
            PhysicsTuningSlider = (RefSpec32)this.PhysicsTuningSlider.Clone(),
            Range = this.Range,
        };
    }
}