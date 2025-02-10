// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:35 AM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(DamageScaleRecord))]
public struct DamageScaleRecord : IComplexType
{
    public float VisualScale;
    public float HitPointScale;

    public void EndianSwap()
    {
        VisualScale = VisualScale.EndianSwap();
        HitPointScale = HitPointScale.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}