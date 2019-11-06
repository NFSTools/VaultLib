// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 3:28 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.EA.WorldMap
{
    [VLTTypeInfo("EA::WorldMap::eWMFeatureType")]
    public enum eWMFeatureType
    {
        WORLDMAP_FEATURE_TYPE_BASICPOINTFEATURE,
        WORLDMAP_FEATURE_TYPE_BASICAREAFEATURE,
        WORLDMAP_FEATURE_TYPE_RACE_EVENT,
        WORLDMAP_FEATURE_TYPE_COP_CARS,
        WORLDMAP_FEATURE_TYPE_LOCAL_PLAYER,
        WORLDMAP_FEATURE_TYPE_REMOTE_PLAYER,
        WORLDMAP_FEATURE_TYPE_POINT_OF_INTEREST,
        WORLDMAP_FEATURE_TYPE_TREASURE,
        WORLDMAP_FEATURE_TYPE_MAX
    }
}