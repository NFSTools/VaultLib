// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:06 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound;

[VltTypeInfo("Sound::SirenSequence")]
public struct SirenSequence : IComplexType
{
    public enum SirenMode
    {
        SIREN_OFF = 0x1,
        SIREN_WAIL = 0x2,
        SIREN_YELP = 0x3,
        SIREN_PRIORITY = 0x4,
        SIREN_HORN = 0x5,
        SIREN_DEATH = 0x6,
        SIREN_INIT = 0x7,
        MAX_SIREN_STATES = 0x8,
    }

    public SirenMode mMode;
    public float mDuration;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}