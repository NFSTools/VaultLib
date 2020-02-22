using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(eSNDCTLSTATE))]
    public enum eSNDCTLSTATE
    {
        SNDSTATE_OFF = 0x0,
        SNDSTATE_PAUSE = 0x1,
        SNDSTATE_INGAME = 0x2,
        SNDSTATE_FE = 0x3,
        SNDSTATE_FE_UPSCREEN = 0x4,
        SNDSTATE_FE_SMS_MESSAGE = 0x5,
        SNDSTATE_NIS_STORY = 0x6,
        SNDSTATE_NIS_INTRO = 0x7,
        SNDSTATE_NIS_321 = 0x8,
        SNDSTATE_NIS_BLK = 0x9,
        SNDSTATE_NIS_ARREST = 0xA,
        SNDSTATE_FMV = 0xB,
        SNDSTATE_STOP_MUSIC = 0xC,
        SNDSTATE_GAMESTARTRACE = 0xD,
        SNDSTATE_MINILOAD = 0xE,
        SNDSTATE_FADEOUT = 0xF,
        SNDSTATE_ERROR = 0x10,
        SNDSTATE_SYSTEM_HUD = 0x11,
        SNDSTATE_NIS_CAM = 0x12,
        SNDSTATE_POSTRACESCREEN = 0x13,
        SNDSTATE_LOADTRANSITION = 0x14,
        SNDSTATE_GENERICDIALOG = 0x15,
        SNDSTATE_NISLOADING = 0x16,
        SNDSTATE_PRERACESCREEN = 0x17,
        MAX_SNDCTL_STATES = 0x18,
    }
}