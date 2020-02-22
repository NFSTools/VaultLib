using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Audio
{
    [VLTTypeInfo("Audio::CornerType")]
    public enum CornerType
    {
        kAudioCorner_Other = 0x0,
        kAudioCorner_First = 0x1,
        kAudioCorner_Last = 0x2,
        kAudioCorner_SecondLast = 0x3,
        kAudioCorner_ThirdLast = 0x4,
    }
}