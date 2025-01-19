// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:07 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(LeaderSupport))]
    public struct LeaderSupport
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
    }
}