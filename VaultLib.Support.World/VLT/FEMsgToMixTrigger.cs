using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT;

[VltTypeInfo(nameof(FEMsgToMixTrigger))]
public struct FEMsgToMixTrigger : IComplexType
{
    public uint Value1;
    public uint Value2;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}