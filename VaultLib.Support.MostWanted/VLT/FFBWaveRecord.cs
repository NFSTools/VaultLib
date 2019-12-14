// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:02 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(FFBWaveRecord))]
    public class FFBWaveRecord : VLTBaseType
    {
        public float Frequency_A { get; set; }
        public float Amplitude_A { get; set; }
        public float Offset_A { get; set; }
        public float Threshold_A { get; set; }
        public float Frequency_B { get; set; }
        public float Amplitude_B { get; set; }
        public float Offset_B { get; set; }
        public float Threshold_B { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Frequency_A = br.ReadSingle();
            Amplitude_A = br.ReadSingle();
            Offset_A = br.ReadSingle();
            Threshold_A = br.ReadSingle();
            Frequency_B = br.ReadSingle();
            Amplitude_B = br.ReadSingle();
            Offset_B = br.ReadSingle();
            Threshold_B = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Frequency_A);
            bw.Write(Amplitude_A);
            bw.Write(Offset_A);
            bw.Write(Threshold_A);
            bw.Write(Frequency_B);
            bw.Write(Amplitude_B);
            bw.Write(Offset_B);
            bw.Write(Threshold_B);
        }

        public FFBWaveRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FFBWaveRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}