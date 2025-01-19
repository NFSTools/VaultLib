// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:07 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(HeavySupport))]
    public struct HeavySupport
    {
        public enum HeavySupportStrategy
        {
            E_BRAKE = 0x1,
            COORDINATED_E_BRAKE = 0x2,
            RAM = 0x3,
            HEAVY_ROADBLOCK = 0x4,
        };

        public HeavySupportStrategy HeavyStrategy;
        public uint Chance;
        public float Duration;
        public uint ChanceBigSUV;
    }
}