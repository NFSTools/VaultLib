using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::EventMode")]
    public enum EventMode
    {
        kEventMode_Unknown = 0,
        kEventMode_TollBooth = 1,
        kEventMode_Canyon = 2,
        kEventMode_Challenge = 3,
        kEventMode_Circuit = 4,
        kEventMode_Drift = 5,
        kEventMode_PursuitKnockOut = 6,
        kEventMode_PursuitTag = 7,
        kEventMode_SpeedTrap = 8,
        kEventMode_Sprint = 9,
        kEventMode_CanyonDrift = 10,
        kEventMode_Pursuit = 12,
        kEventMode_LapKnockOut = 15,
        kEventMode_Drag = 19,
        kEventMode_CoopPursuit = 21,
        kEventMode_MeetingPlace = 22,
        kEventMode_TeamPursuit = 23,
        kEventMode_TeamEscape = 24,
        kEventMode_TreasureHunt = 25,
    }
}
