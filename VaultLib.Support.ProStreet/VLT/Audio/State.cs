using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Audio
{
    [VLTTypeInfo("Audio::State")]
    public enum State
    {
        kAudioState_Unknown = 0x0,
        kAudioState_EnteringCorner = 0x1,
        kAudioState_DuringCorner = 0x2,
        kAudioState_ExitingCorner = 0x3,
    }
}