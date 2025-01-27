using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PhysicsTuningPreset))]
public class PhysicsTuningPreset: VltBaseType<uint>
{
    public RefSpec<uint> PhysicsTuningSlider { get; set; } = new();
    public bool CenteredAroundPreset { get; set; }
    public float Position { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        PhysicsTuningSlider.Read(context, fieldContext, br);
        CenteredAroundPreset = br.ReadBoolean();
        br.SafeAlignReader(4);
        Position = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        PhysicsTuningSlider.Write(context, fieldContext, bw);
        bw.Write(CenteredAroundPreset);
        bw.AlignWriter(4);
        bw.Write(Position);
    }
}