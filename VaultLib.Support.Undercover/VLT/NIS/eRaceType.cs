using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::eRaceType")]
    public enum eRaceType
    {
        RACE_TYPE_SHORT_FOOTPRINT = 0x0,
        RACE_TYPE_MEDIUM_FOOTPRINT = 0x1,
        RACE_TYPE_LONG_FOOTPRINT = 0x2,
        RACE_TYPE_CHECKPOINT = 0x3,
        RACE_TYPE_HIGHWAY_BATTLE = 0x4,
        RACE_TYPE_OUTRUN = 0x5,
        RACE_TYPE_WANTED_EVENT = 0x6,
        RACE_TYPE_INVALID = 0x7,
    }
}