// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 9:15 PM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CarBodyMotion))]
public struct CarBodyMotion : IComplexType
{
    public float DegPerG;
    public float MaxGs;
    public float DegPerSec;

    public void EndianSwap()
    {
        DegPerG = DegPerG.EndianSwap();
        MaxGs = MaxGs.EndianSwap();
        DegPerSec = DegPerSec.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}