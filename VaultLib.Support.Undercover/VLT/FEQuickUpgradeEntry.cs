using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEQuickUpgradeEntry))]
public class FEQuickUpgradeEntry: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public eQuickUpgradePackages Package { get; set; }
    public eQuickUpgradeLevels Level { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Package = br.ReadEnum<eQuickUpgradePackages>();
        Level = br.ReadEnum<eQuickUpgradeLevels>();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Package);
        bw.WriteEnum(Level);
    }
}