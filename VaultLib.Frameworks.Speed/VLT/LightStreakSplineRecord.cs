using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(LightStreakSplineRecord))]
public struct LightStreakSplineRecord : IComplexType
{
    public uint mEnum;
    public uint mIndex;

    public void EndianSwap()
    {
        mEnum = BinaryPrimitives.ReverseEndianness(mEnum);
        mIndex = BinaryPrimitives.ReverseEndianness(mIndex);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}