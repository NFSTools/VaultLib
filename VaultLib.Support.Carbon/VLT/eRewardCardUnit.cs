// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:43 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(eRewardCardUnit))]
    public enum eRewardCardUnit
    {
        UNIT_NONE = 0x0,
        UNIT_TIME = 0x1,
        UNIT_NUMBER = 0x2,
        UNIT_SPEED = 0x3,
    }
}