using System.Buffers.Binary;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PerformanceWeight))]
public struct PerformanceWeight : IComplexType
{
    public ePerformanceType PerformanceType;
    public float Percentage;

    public void EndianSwap()
    {
        PerformanceType = (ePerformanceType)BinaryPrimitives.ReverseEndianness((uint)PerformanceType);
        Percentage = Percentage.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}