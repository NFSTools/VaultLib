using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(BluePrintType))]
    public enum BluePrintType
    {
        BLUEPRINT_GRIP = 0x0,
        BLUEPRINT_DRIFT = 0x1,
        BLUEPRINT_DRAG = 0x2,
        BLUEPRINT_SPEED_CHALLENGE = 0x3,
        BLUEPRINT_FIRST = 0x0,
        BLUEPRINT_LAST = 0x3,
        BLUEPRINT_NUM = 0x4,
    }
}