using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::Difficulty")]
    public enum Difficulty
    {
        kRaceDifficulty_Easy = 0x0,
        kRaceDifficulty_Medium = 0x1,
        kRaceDifficulty_Hard = 0x2,
        kRaceDifficulty_Insane = 0x3,
        kRaceDifficulty_NumDifficulties = 0x4,
    }
}