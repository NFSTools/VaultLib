using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

// TODO: figure this out
[VltTypeInfo(nameof(LightFlash))]
public struct LightFlash : IComplexType
{
    public float Value1;
    public float Value2;

    public void EndianSwap()
    {
        Value1 = Value1.EndianSwap();
        Value2 = Value2.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}