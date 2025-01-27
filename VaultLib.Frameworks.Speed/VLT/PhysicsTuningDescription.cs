using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PhysicsTuningDescription))]
public class PhysicsTuningDescription: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> PhysicsTuning { get; set; } = new();
    public bool Increase { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PhysicsTuning.Read(context, fieldContext, br);
        Increase = br.ReadBoolean();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        PhysicsTuning.Write(context, fieldContext, bw);
        bw.Write(Increase);
        bw.AlignWriter(4);
    }
}