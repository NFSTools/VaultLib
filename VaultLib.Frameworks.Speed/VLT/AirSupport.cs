// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:09 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(AirSupport))]
    public struct AirSupport
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
    }
}