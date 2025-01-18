using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PerformanceWeightWithDirection))]
    public class PerformanceWeightWithDirection : VltBaseType
    {
        public ePerformanceType mPerformanceType { get; set; }
        public bool mInverse { get; set; }
        public float mPercentage { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            mPerformanceType = br.ReadEnum<ePerformanceType>();
            mInverse = br.ReadBoolean();
            br.AlignReader(4);
            mPercentage = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(mPerformanceType);
            bw.Write(mInverse);
            bw.AlignWriter(4);
            bw.Write(mPercentage);
        }
    }
}