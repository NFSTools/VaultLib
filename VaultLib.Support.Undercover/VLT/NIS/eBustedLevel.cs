using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::eBustedLevel")]
    public enum eBustedLevel
    {
        BUST_LEVEL_CIVIC = 0x0,
        BUST_LEVEL_STATE = 0x1,
        BUST_LEVEL_SUPER_STATE = 0x2,
        BUST_LEVEL_FED = 0x3,
        BUST_LEVEL_SUV = 0x4,
        BUST_LEVEL_INVALID = 0x5,
    }
}