// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:55 PM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(GMapCurvePoint))]
public struct GMapCurvePoint : IComplexType
{
    public float X;
    public float Y;

    public void EndianSwap()
    {
        X = X.EndianSwap();
        Y = Y.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}