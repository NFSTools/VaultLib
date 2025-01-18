// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:09 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(AirSupport))]
    public class AirSupport : VltBaseType
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

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            HeliStrategy = br.ReadEnum<AirSupportStrategy>();
            Chance = br.ReadUInt32();
            Duration = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(HeliStrategy);
            bw.Write(Chance);
            bw.Write(Duration);
        }
    }
}