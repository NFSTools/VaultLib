using System.Buffers.Binary;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PerformanceStatsRange))]
public struct PerformanceStatsRange : IComplexType
{
    public ePerformanceType mPerformanceType;
    public float mMin;
    public float mMax;

    public void EndianSwap()
    {
        mPerformanceType = (ePerformanceType)BinaryPrimitives.ReverseEndianness((uint)mPerformanceType);
        mMin = mMin.EndianSwap();
        mMax = mMax.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}