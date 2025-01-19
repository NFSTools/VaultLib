// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:26 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound
{
    [VltTypeInfo("Sound::BattleMusicTuning")]
    public struct BattleMusicTuning
    {
        public float TimeAhead_HiToMed { get; set; }
        public float TimeAhead_MedToLo { get; set; }
        public float TimeBehind_HiToFail { get; set; }
        public float TimeAhead_FailToHi { get; set; }
    }
}