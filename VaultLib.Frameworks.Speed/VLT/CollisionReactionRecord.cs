// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:27 AM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CollisionReactionRecord))]
public struct CollisionReactionRecord : IComplexType
{
    public float Elasticity;
    public float RollHeight;
    public float WeightBias;
    public float MassScale;
    public float StunSpeed;
    public float StunTime;

    public void EndianSwap()
    {
        Elasticity = Elasticity.EndianSwap();
        RollHeight = RollHeight.EndianSwap();
        WeightBias = WeightBias.EndianSwap();
        MassScale = MassScale.EndianSwap();
        StunSpeed = StunSpeed.EndianSwap();
        StunTime = StunTime.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}