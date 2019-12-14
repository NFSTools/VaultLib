// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:09 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(AirSupport))]
    public class AirSupport : VLTBaseType
    {
        public enum AirSupportStrategy
        {
            HI_PATROL = 0x0,
            PURSUIT = 0x1,
            SKID_HIT = 0x2,
            SPIKE_DROP = 0x3,
        }

        public AirSupportStrategy HeliStrategy { get; set; }
        public uint Chance { get; set; }
        public float Duration { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            HeliStrategy = br.ReadEnum<AirSupportStrategy>();
            Chance = br.ReadUInt32();
            Duration = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(HeliStrategy);
            bw.Write(Chance);
            bw.Write(Duration);
        }

        public AirSupport(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public AirSupport(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}