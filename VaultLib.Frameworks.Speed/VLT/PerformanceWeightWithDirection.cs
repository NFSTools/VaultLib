using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PerformanceWeightWithDirection))]
    public class PerformanceWeightWithDirection : VltBaseType
    {
        public PerformanceWeightWithDirection(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PerformanceWeightWithDirection(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public ePerformanceType mPerformanceType { get; set; }
        public bool mInverse { get; set; }
        public float mPercentage { get; set; }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            mPerformanceType = br.ReadEnum<ePerformanceType>();
            mInverse = br.ReadBoolean();
            br.AlignReader(4);
            mPercentage = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.WriteEnum(mPerformanceType);
            bw.Write(mInverse);
            bw.AlignWriter(4);
            bw.Write(mPercentage);
        }
    }
}