// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:07 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(RoadNoiseRecord))]
public struct RoadNoiseRecord
{
    public float Frequency;
    public float Amplitude;
    public float MinSpeed;
    public float MaxSpeed;
}