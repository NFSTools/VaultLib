// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:09 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AirSupport))]
public struct AirSupport : IComplexType
{
    public enum AirSupportStrategy
    {
        HI_PATROL = 0x0,
        PURSUIT = 0x1,
        SKID_HIT = 0x2,
        SPIKE_DROP = 0x3,
    }

    public AirSupportStrategy HeliStrategy;
    public uint Chance;
    public float Duration;

    public void EndianSwap()
    {
        HeliStrategy = (AirSupportStrategy)BinaryPrimitives.ReverseEndianness((uint)HeliStrategy);
        Chance = BinaryPrimitives.ReverseEndianness(Chance);
        Duration = Duration.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}