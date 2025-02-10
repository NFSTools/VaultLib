// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 10:42 AM.

using System.Buffers.Binary;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(EmittedGeometryRecord))]
public struct EmittedGeometryRecord : IComplexType
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