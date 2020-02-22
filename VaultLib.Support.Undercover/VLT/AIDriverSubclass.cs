using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(AIDriverSubclass))]
    public enum AIDriverSubclass
    {
        AIDRIVER_INVALID = -1,
        AIDRIVER_RACER = 0x0,
        AIDRIVER_ALLY = 0x1,
        AIDRIVER_THUG = 0x2,
        AIDRIVER_BOSS = 0x3,
        AIDRIVER_MAX = 0x4,
    }
}