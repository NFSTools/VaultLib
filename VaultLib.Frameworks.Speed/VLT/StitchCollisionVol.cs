using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(StitchCollisionVol))]
public struct StitchCollisionVol : IComplexType
{
    public short Vol1;
    public short Vol2;
    public short Vol3;
    public short Vol4;

    public void EndianSwap()
    {
        Vol1 = BinaryPrimitives.ReverseEndianness(Vol1);
        Vol2 = BinaryPrimitives.ReverseEndianness(Vol2);
        Vol3 = BinaryPrimitives.ReverseEndianness(Vol3);
        Vol4 = BinaryPrimitives.ReverseEndianness(Vol4);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}