using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Support.ProStreet.VLT.Physics;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(PerformanceWeight))]
    public class PerformanceWeight : VLTBaseType
    {
        public PerformanceWeight(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PerformanceWeight(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public ePerformanceType PerformanceType { get; set; }
        public float Percentage { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            PerformanceType = br.ReadEnum<ePerformanceType>();
            Percentage = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(PerformanceType);
            bw.Write(Percentage);
        }
    }
}