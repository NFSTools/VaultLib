// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:07 PM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    public class LeaderSupport : VLTBaseType
    {
        public enum LeaderSupportStrategy
        {
            CROSS_FOLLOW = 0x5,
            CROSS_BRAKE = 0x6,
            CROSS_PLUS_V_BLOCK = 0x7,
        }

        public LeaderSupportStrategy LeaderStrategy { get; set; }
        public uint Chance { get; set; }
        public float Duration { get; set; }
        public uint PriorityChance { get; set; }
        public float PriorityTime { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            LeaderStrategy = br.ReadEnum<LeaderSupportStrategy>();
            Chance = br.ReadUInt32();
            Duration = br.ReadSingle();
            PriorityChance = br.ReadUInt32();
            PriorityTime = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(LeaderStrategy);
            bw.Write(Chance);
            bw.Write(Duration);
            bw.Write(PriorityChance);
            bw.Write(PriorityTime);
        }
    }
}