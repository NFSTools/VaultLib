using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::Mode")]
    public enum Mode
    {
        kRaceMode_None = 0x0,
        kRaceMode_Grip = 0x1,
        kRaceMode_HighSpeedChallenge = 0x2,
        kRaceMode_Drag = 0x3,
        kRaceMode_Drift = 0x4,
        kRaceMode_NumTypes = 0x5,
    }
}