using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PerformanceWeight))]
    public class PerformanceWeight : VltBaseType
    {
        public ePerformanceType PerformanceType { get; set; }
        public float Percentage { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PerformanceType = br.ReadEnum<ePerformanceType>();
            Percentage = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(PerformanceType);
            bw.Write(Percentage);
        }
    }
}