// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:02 AM.

using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FFBWaveRecord))]
public struct FFBWaveRecord : IComplexType
{
    public float Frequency_A;
    public float Amplitude_A;
    public float Offset_A;
    public float Threshold_A;
    public float Frequency_B;
    public float Amplitude_B;
    public float Offset_B;
    public float Threshold_B;

    public void EndianSwap()
    {
        Frequency_A = Frequency_A.EndianSwap();
        Amplitude_A = Amplitude_A.EndianSwap();
        Offset_A = Offset_A.EndianSwap();
        Threshold_A = Threshold_A.EndianSwap();
        Frequency_B = Frequency_B.EndianSwap();
        Amplitude_B = Amplitude_B.EndianSwap();
        Offset_B = Offset_B.EndianSwap();
        Threshold_B = Threshold_B.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}