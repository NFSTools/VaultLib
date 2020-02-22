using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(ePaintUsage))]
    public enum ePaintUsage
    {
        eBodyPaint = 0x0,
        eVinylPaint = 0x1,
        eRimPaint = 0x2,
        eWindowTint = 0x3,
    }
}