using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::HandicapMode")]
    public enum HandicapMode
    {
        kHandicap_None = 0x0,
        kHandicap_Performance = 0x1,
        kHandicap_Group = 0x2,
    }
}