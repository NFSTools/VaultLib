using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(FEMsgToSoundTrigger))]
public struct FEMsgToSoundTrigger : IComplexType
{
    public uint FEngMsg;
    public eMenuSoundTriggers SoundTrigger;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}