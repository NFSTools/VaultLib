using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PhysicsTuningPreset))]
public class PhysicsTuningPreset : VltBaseType
{
    public RefSpec PhysicsTuningSlider { get; set; } = new();
    public bool CenteredAroundPreset { get; set; }
    public float Position { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        PhysicsTuningSlider.Read(context, fieldContext, br);
        CenteredAroundPreset = br.ReadBoolean();
        br.AlignReader(4);
        Position = br.ReadSingle();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        PhysicsTuningSlider.Write(context, fieldContext, bw);
        bw.Write(CenteredAroundPreset);
        bw.AlignWriter(4);
        bw.Write(Position);
    }
}