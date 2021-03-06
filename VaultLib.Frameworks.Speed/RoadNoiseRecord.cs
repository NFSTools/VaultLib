﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:07 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(RoadNoiseRecord))]
    public class RoadNoiseRecord : VLTBaseType
    {
        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public float MinSpeed { get; set; }
        public float MaxSpeed { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Frequency = br.ReadSingle();
            Amplitude = br.ReadSingle();
            MinSpeed = br.ReadSingle();
            MaxSpeed = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Frequency);
            bw.Write(Amplitude);
            bw.Write(MinSpeed);
            bw.Write(MaxSpeed);
        }

        public RoadNoiseRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RoadNoiseRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}