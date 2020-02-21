using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(eTURN_INDICATORS))]
    public enum eTURN_INDICATORS
    {
        kTurnIndicator20 = 0x0,
        kTurnIndicator45 = 0x1,
        kTurnIndicator90 = 0x2,
        kTurnIndicator135 = 0x3,
        kTurnIndicator180 = 0x4,
        kTurnIndicatorS = 0x5,
        kTurnIndicatorChicane = 0x6,
        kNumTurnIndicators = 0x7,
    }
}