using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.GRace
{
    [VLTTypeInfo("GRace::ShortcutType")]
    public enum ShortcutType
    {
        kShortcutType_None = 0x0,
        kShortcutType_ParkRight = 0x1,
        kShortcutType_AlleyRight = 0x2,
        kShortcutType_SideStreetRight = 0x3,
        kShortcutType_TunnelRight = 0x4,
        kShortcutType_GenericRight = 0x5,
        kShortcutType_ParkLeft = 0x6,
        kShortcutType_AlleyLeft = 0x7,
        kShortcutType_SideStreetLeft = 0x8,
        kShortcutType_TunnelLeft = 0x9,
        kShortcutType_GenericLeft = 0xA,
    }
}