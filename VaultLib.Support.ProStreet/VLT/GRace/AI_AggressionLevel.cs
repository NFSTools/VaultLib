using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::AI_AggressionLevel")]
    public enum AI_AggressionLevel
    {
        kAggression_Careful = 0x0,
        kAggression_Intermediate = 0x1,
        kAggression_Aggressive = 0x2,
    }
}