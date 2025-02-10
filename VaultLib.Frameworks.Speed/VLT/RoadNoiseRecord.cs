// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:07 AM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(RoadNoiseRecord))]
public struct RoadNoiseRecord : IComplexType
{
    public float Frequency;
    public float Amplitude;
    public float MinSpeed;
    public float MaxSpeed;

    public void EndianSwap()
    {
        Frequency = Frequency.EndianSwap();
        Amplitude = Amplitude.EndianSwap();
        MinSpeed = MinSpeed.EndianSwap();
        MaxSpeed = MaxSpeed.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}