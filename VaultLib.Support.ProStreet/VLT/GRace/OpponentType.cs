using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::OpponentType")]
    public enum OpponentType
    {
        kOpponentType_Default = 0x0,
        kOpponentType_Challenge = 0x1,
        kOpponentType_Rival = 0x2,
        kOpponentType_King = 0x3,
        kOpponentType_NumTypes = 0x4,
    }
}