// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:44 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(eRewardCardDifficulty))]
    public enum eRewardCardDifficulty
    {
        DIFFICULTY_UNSPECIFIED = -1,
        DIFFICULTY_EASY = 0x0,
        DIFFICULTY_MEDUIM = 0x1,
        DIFFICULTY_HARD = 0x2,
    }
}