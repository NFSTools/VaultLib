using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::Variant")]
    public enum Variant : uint
    {
        kRaceVariant_Invalid = 0xFFFFFFFF,
        kRaceVariant_None = 0x0,
        kRaceVariant_SpeedChallenge = 0x1,
        kRaceVariant_TopSpeed = 0x2,
        kRaceVariant_Circuit = 0x3,
        kRaceVariant_SectorShootout = 0x4,
        kRaceVariant_TimeAttack = 0x5,
        kRaceVariant_Drag = 0x6,
        kRaceVariant_Drag_Wheelie = 0x7,
        kRaceVariant_Drift_Solo = 0x8,
        kRaceVariant_Drift_Race = 0x9,
        kRaceVariant_Drift_Tandem = 0xA,
        kRaceVariant_NumTypes = 0xB,
    }
}