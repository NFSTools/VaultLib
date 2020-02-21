using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::Type")]
    public enum Type
    {
        kRaceType_None = -1,
        kRaceType_P2P,
        kRaceType_Circuit,
        kRaceType_Drag,
        kRaceType_Knockout,
        kRaceType_Tollbooth,
        kRaceType_SpeedTrap,
        kRaceType_Checkpoint,
        kRaceType_CashGrab,
        kRaceType_Challenge,
        kRaceType_JumpToSpeedTrap,
        kRaceType_JumpToMilestone,
        kRaceType_Drift,
        kRaceType_PursuitTag,
        kRaceType_PursuitKnockout,
        kRaceType_Encounter,
        kRaceType_NumTypes
    }
}
