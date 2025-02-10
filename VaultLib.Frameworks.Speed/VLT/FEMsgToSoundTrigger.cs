using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FEMsgToSoundTrigger))]
public struct FEMsgToSoundTrigger : IComplexType
{
    public uint FEngMsg;
    public eMenuSoundTriggers SoundTrigger;

    public void EndianSwap()
    {
        FEngMsg = BinaryPrimitives.ReverseEndianness(FEngMsg);
        SoundTrigger = (eMenuSoundTriggers)BinaryPrimitives.ReverseEndianness((uint)SoundTrigger);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}