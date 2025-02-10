using System.Buffers.Binary;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FECarPartInfo))]
public struct FECarPartInfo : IComplexType
{
    public eFEPartUpgradeLevels Level;
    public float Cost;

    public void EndianSwap()
    {
        Level = (eFEPartUpgradeLevels)BinaryPrimitives.ReverseEndianness((uint)Level);
        Cost = Cost.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}