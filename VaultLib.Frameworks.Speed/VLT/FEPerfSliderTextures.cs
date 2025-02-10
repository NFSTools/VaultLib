using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEPerfSliderTextures))]
public struct FEPerfSliderTextures : IComplexType
{
    public eFEPartUpgradeLevels Level;
    public uint Name1;
    public uint Name2;
    public uint Name3;
    public uint Logo1;
    public uint Logo2;
    public uint Logo3;
    public uint Logo4;
    public uint Logo5;
    public uint Logo6;
    public uint Logo7;
    public uint Logo8;
    public uint Logo9;

    public void EndianSwap()
    {
        Level = (eFEPartUpgradeLevels)BinaryPrimitives.ReverseEndianness((uint)Level);
        Name1 = BinaryPrimitives.ReverseEndianness((uint)Name1);
        Name2 = BinaryPrimitives.ReverseEndianness((uint)Name2);
        Name3 = BinaryPrimitives.ReverseEndianness((uint)Name3);
        Logo1 = BinaryPrimitives.ReverseEndianness((uint)Logo1);
        Logo2 = BinaryPrimitives.ReverseEndianness((uint)Logo2);
        Logo3 = BinaryPrimitives.ReverseEndianness((uint)Logo3);
        Logo4 = BinaryPrimitives.ReverseEndianness((uint)Logo4);
        Logo5 = BinaryPrimitives.ReverseEndianness((uint)Logo5);
        Logo6 = BinaryPrimitives.ReverseEndianness((uint)Logo6);
        Logo7 = BinaryPrimitives.ReverseEndianness((uint)Logo7);
        Logo8 = BinaryPrimitives.ReverseEndianness((uint)Logo8);
        Logo9 = BinaryPrimitives.ReverseEndianness((uint)Logo9);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}