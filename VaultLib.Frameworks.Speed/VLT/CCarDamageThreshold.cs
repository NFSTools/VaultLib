using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CCarDamageThreshold))]
public struct CCarDamageThreshold : IComplexType
{
    public float Threshold0;
    public float Threshold1;
    public float Threshold2;
    public float Threshold3;
    public float DeltaThreshold0;
    public float DeltaThreshold1;
    public float DeltaThreshold2;
    public float DeltaThreshold3;

    public void EndianSwap()
    {
        Threshold0 = Threshold0.EndianSwap();
        Threshold1 = Threshold1.EndianSwap();
        Threshold2 = Threshold2.EndianSwap();
        Threshold3 = Threshold3.EndianSwap();
        DeltaThreshold0 = DeltaThreshold0.EndianSwap();
        DeltaThreshold1 = DeltaThreshold1.EndianSwap();
        DeltaThreshold2 = DeltaThreshold2.EndianSwap();
        DeltaThreshold3 = DeltaThreshold3.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}