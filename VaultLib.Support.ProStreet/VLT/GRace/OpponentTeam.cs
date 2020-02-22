using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::OpponentTeam")]
    public enum OpponentTeam
    {
        kOpponentTeam_Default = 0x0,
        kOpponentTeam_ApexGlide = 0x1,
        kOpponentTeam_AfterMix = 0x2,
        kOpponentTeam_TougeUnion = 0x3,
        kOpponentTeam_GripRunners = 0x4,
        kOpponentTeam_Boxcut = 0x5,
        kOpponentTeam_LaterFire = 0x6,
        kOpponentTeam_NumTeams = 0x7,
    }
}