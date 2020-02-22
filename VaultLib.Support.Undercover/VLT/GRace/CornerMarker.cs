using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.GRace
{
    [VLTTypeInfo("GRace::CornerMarker")]
    public enum CornerMarker
    {
        kCornerMarker_MediumLeft = 0x0,
        kCornerMarker_HardLeft = 0x1,
        kCornerMarker_MediumRight = 0x2,
        kCornerMarker_HardRight = 0x3,
        kCornerMarker_ZigZag = 0x4,
    }
}