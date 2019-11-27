// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 4:02 PM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.PerformanceUpgrades
{
    [VLTTypeInfo("PerformanceUpgrades::PartZone")]
    public enum PartZone
    {
        PERFSLOT_ENGINE,
        PERFSLOT_FORCED_INDUCTION,
        PERFSLOT_TRANSMISSION,
        PERFSLOT_SUSPENSION,
        PERFSLOT_BRAKES,
        PERFSLOT_TIRES,
        PERFSLOT_MISC
    }
}