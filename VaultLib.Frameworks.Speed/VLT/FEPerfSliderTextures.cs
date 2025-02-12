using System.Buffers.Binary;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEPerfSliderTextures))]
public struct FEPerfSliderTextures : IComplexType
{
    public eFEPartUpgradeLevels Level;
    public BinKey32 Name1;
    public BinKey32 Name2;
    public BinKey32 Name3;
    public BinKey32 Logo1;
    public BinKey32 Logo2;
    public BinKey32 Logo3;
    public BinKey32 Logo4;
    public BinKey32 Logo5;
    public BinKey32 Logo6;
    public BinKey32 Logo7;
    public BinKey32 Logo8;
    public BinKey32 Logo9;

    public void EndianSwap()
    {
        Level = (eFEPartUpgradeLevels)BinaryPrimitives.ReverseEndianness((uint)Level);
        Name1 = new BinKey32(BinaryPrimitives.ReverseEndianness(Name1.Hash));
        Name2 = new BinKey32(BinaryPrimitives.ReverseEndianness(Name2.Hash));
        Name3 = new BinKey32(BinaryPrimitives.ReverseEndianness(Name3.Hash));
        Logo1 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo1.Hash));
        Logo2 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo2.Hash));
        Logo3 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo3.Hash));
        Logo4 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo4.Hash));
        Logo5 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo5.Hash));
        Logo6 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo6.Hash));
        Logo7 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo7.Hash));
        Logo8 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo8.Hash));
        Logo9 = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo9.Hash));
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}