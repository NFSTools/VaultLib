using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEQuickUpgradeEntry))]
public class FEQuickUpgradeEntry: VltBaseType<uint>
{
    public eQuickUpgradePackages Package { get; set; }
    public eQuickUpgradeLevels Level { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Package = br.ReadEnum<eQuickUpgradePackages>();
        Level = br.ReadEnum<eQuickUpgradeLevels>();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Package);
        bw.WriteEnum(Level);
    }
}