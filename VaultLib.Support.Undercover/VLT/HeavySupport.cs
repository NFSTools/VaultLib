// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:07 PM.

using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(HeavySupport))]
    public class HeavySupport : VLTBaseType
    {
        public enum HeavySupportStrategy
        {
            E_BRAKE = 0x1,
            COORDINATED_E_BRAKE = 0x2,
            RAM = 0x3,
            HEAVY_ROADBLOCK = 0x4,
        };

        public HeavySupportStrategy HeavyStrategy { get; set; }
        public uint Chance { get; set; }
        public float Duration { get; set; }
        public uint ChanceBigSUV { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            HeavyStrategy = br.ReadEnum<HeavySupportStrategy>();
            Chance = br.ReadUInt32();
            Duration = br.ReadSingle();
            ChanceBigSUV = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(HeavyStrategy);
            bw.Write(Chance);
            bw.Write(Duration);
            bw.Write(ChanceBigSUV);
        }

        public HeavySupport(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public HeavySupport(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}