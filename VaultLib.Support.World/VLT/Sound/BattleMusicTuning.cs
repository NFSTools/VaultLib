// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 7:26 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound;

[VltTypeInfo("Sound::BattleMusicTuning")]
public struct BattleMusicTuning : IComplexType
{
    public float TimeAhead_HiToMed;
    public float TimeAhead_MedToLo;
    public float TimeBehind_HiToFail;
    public float TimeAhead_FailToHi;
    
    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}