// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:30 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.EA.WorldMap
{
    [VLTTypeInfo("EA::WorldMap::POIType")]
    public enum POIType
    {
        POIType_Unknown = -1,
        POIType_Cooldown,
        POIType_PursuitBreaker,
        POIType_FinishLine,
        POIType_Treasure,
        POIType_TreasureArea
    }
}