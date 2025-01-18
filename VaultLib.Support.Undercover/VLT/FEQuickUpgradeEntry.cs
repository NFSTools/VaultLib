using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(FEQuickUpgradeEntry))]
    public class FEQuickUpgradeEntry : VltBaseType
    {
        public FEQuickUpgradeEntry(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEQuickUpgradeEntry(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eQuickUpgradePackages Package { get; set; }
        public eQuickUpgradeLevels Level { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Package = br.ReadEnum<eQuickUpgradePackages>();
            Level = br.ReadEnum<eQuickUpgradeLevels>();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.WriteEnum(Package);
            bw.WriteEnum(Level);
        }
    }
}