// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:06 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(EngineLFOParams))]
    public class EngineLFOParams : VLTBaseType
    {
        public float frequency_start { get; set; }
        public float frequency_end { get; set; }
        public float RPM_amplitude { get; set; }
        public float vol_amplitude { get; set; }
        public float lifespan { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            frequency_start = br.ReadSingle();
            frequency_end = br.ReadSingle();
            RPM_amplitude = br.ReadSingle();
            vol_amplitude = br.ReadSingle();
            lifespan = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(frequency_start);
            bw.Write(frequency_end);
            bw.Write(RPM_amplitude);
            bw.Write(vol_amplitude);
            bw.Write(lifespan);
        }

        public EngineLFOParams(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public EngineLFOParams(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}