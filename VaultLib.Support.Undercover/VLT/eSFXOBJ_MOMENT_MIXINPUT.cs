// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 3:42 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(eSFXOBJ_MOMENT_MIXINPUT))]
    public enum eSFXOBJ_MOMENT_MIXINPUT
    {
        eTRG_MOMENT_SPEED_BREAKER = 0x0,
        eTRG_MOMENT_PURSUIT_BREAKER = 0x1,
        eTRG_MOMENT_NIS = 0x2,
        eTRG_MOMENT_CRASH_SWEETNER = 0x3,
    }
}