using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.NIS
{
    [VLTTypeInfo("NIS::ePIP_EVENT_CATEGORY")]
    public enum ePIP_EVENT_CATEGORY
    {
        UNCATEGORIZED = 0x0,
        CRASH_EVENT = 0x1,
        CANYON_SCORE_EVENT = 0x2,
        PASSING_EVENT = 0x3,
        NUM_PIP_EVENT_CATEGORIES = 0x4,
    }
}