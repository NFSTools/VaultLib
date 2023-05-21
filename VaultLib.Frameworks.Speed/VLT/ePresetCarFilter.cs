using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(ePresetCarFilter))]
    public enum ePresetCarFilter
    {
        PRESET_CAR_FILTER_BONUS = 0x0,
        PRESET_CAR_FILTER_CUSTOM = 0x1,
        PRESET_CAR_FILTER_AI = 0x2,
        PRESET_CAR_FILTER_DEBUG = 0x3,
    }
}