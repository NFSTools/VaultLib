using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(FEQuickUpgradeEntry))]
    public class FEQuickUpgradeEntry : VLTBaseType
    {
        public FEQuickUpgradeEntry(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEQuickUpgradeEntry(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eQuickUpgradePackages Package { get; set; }
        public eQuickUpgradeLevels Level { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Package = br.ReadEnum<eQuickUpgradePackages>();
            Level = br.ReadEnum<eQuickUpgradeLevels>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(Package);
            bw.WriteEnum(Level);
        }
    }
}