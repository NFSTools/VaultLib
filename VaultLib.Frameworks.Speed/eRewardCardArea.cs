// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:43 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(eRewardCardArea))]
    public enum eRewardCardArea
    {
        REWARD_CARD_AREA_UNSPECIFIED = -1,
        REWARD_CARD_AREA_OFFLINE = 0x0,
        REWARD_CARD_AREA_ONLINE = 0x1,
        REWARD_CARD_AREA_ACHEIVEMENTS = 0x2,
        NUM_REWARD_CARD_AREAS = 0x3,
    }
}