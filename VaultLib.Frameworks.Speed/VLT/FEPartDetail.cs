// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:59 PM.

using System.Buffers.Binary;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("DUMMY_FEPartDetail")]
public struct FEPartDetail : IComplexType
{
    public BinKey32 Logo;
    public BinKey32 Name;

    public void EndianSwap()
    {
        Logo = new BinKey32(BinaryPrimitives.ReverseEndianness(Logo.Hash));
        Name = new BinKey32(BinaryPrimitives.ReverseEndianness(Name.Hash));
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}