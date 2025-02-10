using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Types;

public struct Vector2 : IComplexType
{
    public float X;
    public float Y;

    public void EndianSwap()
    {
        X = X.EndianSwap();
        Y = Y.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}