// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:59 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("DUMMY_FEPartDetail")]
public struct FEPartDetail : IComplexType
{
    public uint Logo;
    public uint Name;

    public void EndianSwap()
    {
        Logo = BinaryPrimitives.ReverseEndianness(Logo);
        Name = BinaryPrimitives.ReverseEndianness(Name);
    }
}