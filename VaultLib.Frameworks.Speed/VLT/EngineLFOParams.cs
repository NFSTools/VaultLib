// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:06 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(EngineLFOParams))]
    public struct EngineLFOParams
    {
        public float frequency_start;
        public float frequency_end;
        public float RPM_amplitude;
        public float vol_amplitude;
        public float lifespan;
    }
}