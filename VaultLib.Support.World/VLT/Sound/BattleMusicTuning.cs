// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:26 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound
{
    [VLTTypeInfo("Sound::BattleMusicTuning")]
    public class BattleMusicTuning : VLTBaseType
    {
        public float TimeAhead_HiToMed { get; set; }
        public float TimeAhead_MedToLo { get; set; }
        public float TimeBehind_HiToFail { get; set; }
        public float TimeAhead_FailToHi { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            TimeAhead_HiToMed = br.ReadSingle();
            TimeAhead_MedToLo = br.ReadSingle();
            TimeBehind_HiToFail = br.ReadSingle();
            TimeAhead_FailToHi = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(TimeAhead_HiToMed);
            bw.Write(TimeAhead_MedToLo);
            bw.Write(TimeBehind_HiToFail);
            bw.Write(TimeAhead_FailToHi);
        }

        public BattleMusicTuning(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public BattleMusicTuning(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}