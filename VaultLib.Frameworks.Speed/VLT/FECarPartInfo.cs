using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FECarPartInfo))]
    public class FECarPartInfo : VltBaseType
    {
        public FECarPartInfo(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FECarPartInfo(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eFEPartUpgradeLevels Level { get; set; }
        public float Cost { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Level = br.ReadEnum<eFEPartUpgradeLevels>();
            Cost = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(Level);
            bw.Write(Cost);
        }
    }
}