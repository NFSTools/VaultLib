// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:01 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(GMapTriangle))]
public struct GMapTriangle : IComplexType
{
    public ushort mPoint1;
    public ushort mPoint2;
    public ushort mPoint3;

    public void EndianSwap()
    {
        mPoint1 = BinaryPrimitives.ReverseEndianness(mPoint1);
        mPoint2 = BinaryPrimitives.ReverseEndianness(mPoint2);
        mPoint3 = BinaryPrimitives.ReverseEndianness(mPoint3);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}