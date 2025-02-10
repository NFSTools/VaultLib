// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:54 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(GMapCurve))]
public struct GMapCurve : IComplexType
{
    public ushort mPointStart;
    public ushort mPointCount;

    public void EndianSwap()
    {
        mPointStart = BinaryPrimitives.ReverseEndianness(mPointStart);
        mPointCount = BinaryPrimitives.ReverseEndianness(mPointCount);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}