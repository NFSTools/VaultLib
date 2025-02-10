using System.Buffers.Binary;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(ParticleTextureRecord))]
public struct ParticleTextureRecord : IComplexType
{
    public BinKey32 mEnum;
    public uint mIndex;

    public void EndianSwap()
    {
        mEnum = new BinKey32(BinaryPrimitives.ReverseEndianness(mEnum.Hash));
        mIndex = BinaryPrimitives.ReverseEndianness(mIndex);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}