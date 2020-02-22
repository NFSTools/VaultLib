// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/09/2019 @ 9:54 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.GRace
{
    [VLTTypeInfo("GRace::Tier")]
    public enum Tier
    {
        kRaceTier_None = 0x0,
        kRaceTier_1 = 0x1,
        kRaceTier_2 = 0x2,
        kRaceTier_3 = 0x3,
        kRaceTier_NumTiers = 0x4,
    }
}