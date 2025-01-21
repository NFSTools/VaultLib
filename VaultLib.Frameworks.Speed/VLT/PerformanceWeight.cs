using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PerformanceWeight))]
public struct PerformanceWeight
{
    public ePerformanceType PerformanceType;
    public float Percentage;
}