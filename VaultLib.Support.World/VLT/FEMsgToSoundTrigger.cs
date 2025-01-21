using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(FEMsgToSoundTrigger))]
public struct FEMsgToSoundTrigger
{
    public uint FEngMsg;
    public eMenuSoundTriggers SoundTrigger;
}