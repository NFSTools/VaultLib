using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(eTURN_DIRECTION))]
    public enum eTURN_DIRECTION
    {
        kTurnIndicatorRight = 0x0,
        kTurnIndicatorLeft = 0x1,
        kNumTurnIndicatorDirections = 0x2,
    }
}