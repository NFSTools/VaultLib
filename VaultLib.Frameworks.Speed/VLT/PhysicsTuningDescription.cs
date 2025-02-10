using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PhysicsTuningDescription))]
public class PhysicsTuningDescription : VltBaseType<Core.DataInterfaces.Key32>
{
    public RefSpec32 PhysicsTuning { get; set; } = new();
    public bool Increase { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        PhysicsTuning.Read(context, fieldContext, br);
        Increase = br.ReadBoolean();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        PhysicsTuning.Write(context, fieldContext, bw);
        bw.Write(Increase);
        bw.AlignWriter(4);
    }

    public override object Clone()
    {
        return new PhysicsTuningDescription
        {
            PhysicsTuning = (RefSpec32)this.PhysicsTuning.Clone(),
            Increase = this.Increase,
        };
    }
}