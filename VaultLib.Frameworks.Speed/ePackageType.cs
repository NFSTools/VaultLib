using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(ePackageType))]
    public enum ePackageType
    {
        ePT_QUICK_POWER = 0x0,
        ePT_QUICK_HANDLING = 0x1,
        ePT_QUICK_VISUAL = 0x2,
        ePT_CUSTOM_ENGINE_CORE = 0x3,
        ePT_CUSTOM_ENGINE_MANAGEMENT = 0x4,
        ePT_CUSTOM_ENGINE_EXHAUST = 0x5,
        ePT_CUSTOM_ENGINE_FORCED_INDUCTION = 0x6,
        ePT_CUSTOM_ENGINE_NITROUS = 0x7,
        ePT_CUSTOM_DRIVETRAIN = 0x8,
        ePT_CUSTOM_SUSPENSION = 0x9,
        ePT_CUSTOM_BRAKES = 0xA,
        ePT_MAX = 0xB,
    }
}