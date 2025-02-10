// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:56 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(GMapCurveRef))]
public struct GMapCurveRef : IComplexType
{
    public enum GMapCurveRefFlags : ushort
    {
        kFlag_Reversed = 1,
    }

    public ushort mCurveIndex;
    public GMapCurveRefFlags Flags;

    public void EndianSwap()
    {
        mCurveIndex = BinaryPrimitives.ReverseEndianness(mCurveIndex);
        Flags = (GMapCurveRefFlags)BinaryPrimitives.ReverseEndianness((ushort)Flags);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}