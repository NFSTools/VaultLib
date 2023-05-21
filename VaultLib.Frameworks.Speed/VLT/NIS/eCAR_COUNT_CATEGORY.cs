using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.NIS
{
    [VLTTypeInfo("NIS::eCAR_COUNT_CATEGORY")]
    public enum eCAR_COUNT_CATEGORY
    {
        ONE = 0x0,
        TWO = 0x1,
        THREE_TO_SIX = 0x2,
        FOUR = 0x3,
        SEVEN_TO_TWELVE = 0x4,
        ANY_CAR_COUNT = 0x5,
    }
}