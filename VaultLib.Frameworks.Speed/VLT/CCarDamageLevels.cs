using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CCarDamageLevels))]
public struct CCarDamageLevels : IComplexType
{
    public float Speed0;
    public float Speed1;
    public float Speed2;
    public float Influence;
    public float LightCrack;
    public float LightShatter;
    public float WindowCrack;
    public float WindowShatter;

    public void EndianSwap()
    {
        Speed0 = Speed0.EndianSwap();
        Speed1 = Speed1.EndianSwap();
        Speed2 = Speed2.EndianSwap();
        Influence = Influence.EndianSwap();
        LightCrack = LightCrack.EndianSwap();
        LightShatter = LightShatter.EndianSwap();
        WindowCrack = WindowCrack.EndianSwap();
        WindowShatter = WindowShatter.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}