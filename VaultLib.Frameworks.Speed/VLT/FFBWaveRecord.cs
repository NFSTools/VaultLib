// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:02 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FFBWaveRecord))]
    public struct FFBWaveRecord
    {
        public float Frequency_A;
        public float Amplitude_A;
        public float Offset_A;
        public float Threshold_A;
        public float Frequency_B;
        public float Amplitude_B;
        public float Offset_B;
        public float Threshold_B;
    }
}