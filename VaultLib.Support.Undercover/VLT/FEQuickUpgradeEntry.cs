using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEQuickUpgradeEntry))]
public class FEQuickUpgradeEntry: VltBaseType<Core.DataInterfaces.Key32>
{
    public eQuickUpgradePackages Package { get; set; }
    public eQuickUpgradeLevels Level { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Package = br.ReadEnum<eQuickUpgradePackages>();
        Level = br.ReadEnum<eQuickUpgradeLevels>();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Package);
        bw.WriteEnum(Level);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}