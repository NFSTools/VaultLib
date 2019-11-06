using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_P_Breaker_Type")]
    public enum Type_P_Breaker_Type
    {
        Invalid_Type_P_Breaker_Type = 0x0,
        Type_P_Breaker_Type_PURS_BRKR_SIGN = 0x1,
        Type_P_Breaker_Type_PURS_BRKR_STATUE = 0x2,
        Type_P_Breaker_Type_PURS_BRKR_MISC = 0x4,
    }
}
