// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:45 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(eRewardCardConditionModifyer))]
    public enum eRewardCardConditionModifyer
    {
        CONDITION_UNSPECIFIED = -1,
        CONDITION_LESS_THAN = 0x0,
        CONDITION_GREATER_THAN = 0x1,
        CONDITION_EQUAL_TO = 0x2,
    }
}