using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::AI_MistakeLevel")]
    public enum AI_MistakeLevel
    {
        kMistake_Rookie = 0x0,
        kMistake_Intermediate = 0x1,
        kMistake_Veteran = 0x2,
    }
}