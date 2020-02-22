using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::OpponentGender")]
    public enum OpponentGender
    {
        kOpponentGender_Male = 0x0,
        kOpponentGender_Female = 0x1,
        kOpponentGender_Ambiguous = 0x2,
        kOpponentType_NumGenders = 0x3,
    }
}