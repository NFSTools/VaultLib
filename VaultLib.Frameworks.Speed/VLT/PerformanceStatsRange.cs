using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PerformanceStatsRange))]
    public struct PerformanceStatsRange
    {
        public ePerformanceType mPerformanceType;
        public float mMin;
        public float mMax;
    }
}