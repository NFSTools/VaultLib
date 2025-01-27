using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(UpgradeSpecs))]
public class UpgradeSpecs: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> ReferencedRow { get; set; } = new();

    public uint UpgradeLevel { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        ReferencedRow.Read(context, fieldContext, br);
        UpgradeLevel = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        ReferencedRow.Write(context, fieldContext, bw);
        bw.Write(UpgradeLevel);
    }
}