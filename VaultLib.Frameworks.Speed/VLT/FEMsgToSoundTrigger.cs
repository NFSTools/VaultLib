using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEMsgToSoundTrigger))]
public struct FEMsgToSoundTrigger
{
    public uint FEngMsg;
    public eMenuSoundTriggers SoundTrigger;
}