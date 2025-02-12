using System.Buffers.Binary;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEDefaultUnlockable))]
public struct FEDefaultUnlockable : IComplexType
{
    public eUnlockableEntity UnlockType;
    public BinKey32 UnlockName;
    public int UnlockLevel;
    public int UnlockTier;

    public void EndianSwap()
    {
        UnlockType = (eUnlockableEntity)BinaryPrimitives.ReverseEndianness((uint)UnlockType);
        UnlockName = new BinKey32(BinaryPrimitives.ReverseEndianness(UnlockName.Hash));
        UnlockLevel = BinaryPrimitives.ReverseEndianness(UnlockLevel);
        UnlockTier = BinaryPrimitives.ReverseEndianness(UnlockTier);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}