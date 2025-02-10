using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PhysicsTuningPreset))]
public class PhysicsTuningPreset : VltBaseType<Core.DataInterfaces.Key32>
{
    public RefSpec32 PhysicsTuningSlider { get; set; } = new();
    public bool CenteredAroundPreset { get; set; }
    public float Position { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PhysicsTuningSlider.Read(context, fieldContext, br);
        CenteredAroundPreset = br.ReadBoolean();
        br.SafeAlignReader(4);
        Position = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        PhysicsTuningSlider.Write(context, fieldContext, bw);
        bw.Write(CenteredAroundPreset);
        bw.AlignWriter(4);
        bw.Write(Position);
    }

    public override object Clone()
    {
        return new PhysicsTuningPreset
        {
            PhysicsTuningSlider = (RefSpec32)this.PhysicsTuningSlider.Clone(),
            CenteredAroundPreset = this.CenteredAroundPreset,
            Position = this.Position,
        };
    }
}