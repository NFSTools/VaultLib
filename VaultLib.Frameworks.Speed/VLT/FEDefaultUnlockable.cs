using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEDefaultUnlockable))]
public struct FEDefaultUnlockable : IComplexType
{
    public eUnlockableEntity UnlockType;
    public uint UnlockName;
    public int UnlockLevel;
    public int UnlockTier;

    public void EndianSwap()
    {
        UnlockType = (eUnlockableEntity)BinaryPrimitives.ReverseEndianness((uint)UnlockType);
        UnlockName = BinaryPrimitives.ReverseEndianness(UnlockName);
        UnlockLevel = BinaryPrimitives.ReverseEndianness(UnlockLevel);
        UnlockTier = BinaryPrimitives.ReverseEndianness(UnlockTier);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}