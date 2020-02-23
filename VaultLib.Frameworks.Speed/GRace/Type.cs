using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.GRace
{
    [VLTTypeInfo("GRace::Type")]
    public enum Type
    {
        kRaceType_None = -1,
        kRaceType_P2P = 0,
        kRaceType_Circuit = 1,
        kRaceType_Drag = 2,
        kRaceType_Knockout = 3,
        kRaceType_Tollbooth = 4,
        kRaceType_SpeedTrap = 5,
        kRaceType_Checkpoint = 6,
        kRaceType_CashGrab = 7,
        kRaceType_Challenge = 8,
        kRaceType_JumpToSpeedTrap = 9,
        kRaceType_JumpToMilestone = 10,
        kRaceType_Drift = 11,
        kRaceType_PursuitTag = 12,
        kRaceType_PursuitKnockout = 13,
        kRaceType_Encounter = 14,
        kRaceType_NumTypes = 15,
    }
}
