using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Support.Undercover.VLT.Physics;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(PerformanceStatsRange))]
    public class PerformanceStatsRange : VLTBaseType
    {
        public PerformanceStatsRange(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PerformanceStatsRange(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public ePerformanceType mPerformanceType { get; set; }
        public float mMin { get; set; }
        public float mMax { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            mPerformanceType = br.ReadEnum<ePerformanceType>();
            mMin = br.ReadSingle();
            mMax = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(mPerformanceType);
            bw.Write(mMin);
            bw.Write(mMax);
        }
    }
}