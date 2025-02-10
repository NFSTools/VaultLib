// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:06 AM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(EngineLFOParams))]
public struct EngineLFOParams : IComplexType
{
    public float frequency_start;
    public float frequency_end;
    public float RPM_amplitude;
    public float vol_amplitude;
    public float lifespan;

    public void EndianSwap()
    {
        frequency_start = frequency_start.EndianSwap();
        frequency_end = frequency_end.EndianSwap();
        RPM_amplitude = RPM_amplitude.EndianSwap();
        vol_amplitude = vol_amplitude.EndianSwap();
        lifespan = lifespan.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}