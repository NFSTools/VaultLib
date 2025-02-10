// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:07 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(LeaderSupport))]
public struct LeaderSupport : IComplexType
{
    public enum LeaderSupportStrategy
    {
        CROSS_FOLLOW = 0x5,
        CROSS_BRAKE = 0x6,
        CROSS_PLUS_V_BLOCK = 0x7,
    }

    public LeaderSupportStrategy LeaderStrategy;
    public uint Chance;
    public float Duration;
    public uint PriorityChance;
    public float PriorityTime;

    public void EndianSwap()
    {
        LeaderStrategy = (LeaderSupportStrategy)BinaryPrimitives.ReverseEndianness((uint)LeaderStrategy);
        Chance = BinaryPrimitives.ReverseEndianness(Chance);
        Duration = Duration.EndianSwap();
        PriorityChance = BinaryPrimitives.ReverseEndianness(PriorityChance);
        PriorityTime = PriorityTime.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}