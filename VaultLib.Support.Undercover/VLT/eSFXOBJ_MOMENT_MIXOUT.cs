// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:32 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(eSFXOBJ_MOMENT_MIXOUT))]
    public enum eSFXOBJ_MOMENT_MIXOUT
    {
        eAZI_MOMENT_3DPOS = 0x0,
        eVOL_MOMENT_PURSUIT_BREAKER = 0x1,
        eFLT_MOMENT_LOWPASSROLLOFF = 0x2,
        eVOL_MOMENT_NIS = 0x3,
        eVOL_MOMENT_CRASH_SWEETNER = 0x4,
        eVOL_MOMENT_SPEED_BREAKER = 0x5,
        eVOL_MOMENT_SPOT_AMBIENCE = 0x6,
        eVOL_MOMENT_MISC_FX = 0x7,
    }
}