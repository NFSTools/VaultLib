using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(UpgradeSpecs))]
public class UpgradeSpecs : VltBaseType
{
    public RefSpec ReferencedRow { get; set; } = new();

    public uint UpgradeLevel { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        ReferencedRow.Read(context, fieldContext, br);
        UpgradeLevel = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        ReferencedRow.Write(context, fieldContext, bw);
        bw.Write(UpgradeLevel);
    }
}