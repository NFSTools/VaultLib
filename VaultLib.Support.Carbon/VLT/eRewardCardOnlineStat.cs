// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:44 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(eRewardCardOnlineStat))]
    public enum eRewardCardOnlineStat
    {
        UNKNOWN_STATS = -1,
        PLAYER_STATS = 0x0,
        GLOBAL_STATS = 0x1,
        OFFLINE_STATS = 0x2,
    }
}