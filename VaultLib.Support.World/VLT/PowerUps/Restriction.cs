using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.PowerUps
{
    [VLTTypeInfo("PowerUps::Restriction")]
    public enum Restriction
    {
        kPowerupRestriction_None,
        kPowerupRestriction_NobodyHasFinishedEvent,
        kPowerupRestriction_IAmNotInFirstPlace,
        kPowerupRestriction_IAmOnTheLastLap,
        kPowerupRestriction_IHaveAFlatTire,
        kPowerupRestriction_IHaveAtLeastOnePowerupRecharging,
        kPowerupRestriction_IAmInPursuitCooldown,
        kPowerupRestriction_IAmNotReversing,
        kPowerupRestriction_IAmInARace,
        kPowerupRestriction_IAmInAPursuit,
        kPowerupRestriction_IAmNotNOSing,
        kPowerupRestriction_IAmNotAutoDriving,
        kPowerupRestriction_ICanOnlyUseThisOncePerRace,
        kPowerupRestriction_WeCanOnlyUseThisOncePerRace,
        kPowerupRestriction_ThisPowerupIsNotCurrentlyActive,
    }
}
