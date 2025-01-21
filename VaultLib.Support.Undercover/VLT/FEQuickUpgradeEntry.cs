using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEQuickUpgradeEntry))]
public class FEQuickUpgradeEntry : VltBaseType
{
    public eQuickUpgradePackages Package { get; set; }
    public eQuickUpgradeLevels Level { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        Package = br.ReadEnum<eQuickUpgradePackages>();
        Level = br.ReadEnum<eQuickUpgradeLevels>();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Package);
        bw.WriteEnum(Level);
    }
}