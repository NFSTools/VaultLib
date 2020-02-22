using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.GRace
{
    [VLTTypeInfo("GRace::MissionType")]
    public enum MissionType
    {
        kMissionType_Wheelman = 0x0,
        kMissionType_HotCar = 0x1,
        kMissionType_Chasedown = 0x2,
    }
}