using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.GRace
{
    [VLTTypeInfo("GRace::NISLength")]
    public enum NISLength
    {
        kNIS_DontCare = 0x0,
        kNIS_Small = 0x1,
        kNIS_Medium = 0x2,
        kNIS_Large = 0x3,
    }
}