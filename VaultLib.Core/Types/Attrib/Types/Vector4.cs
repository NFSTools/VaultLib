using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Types;

public struct Vector4 : IComplexType
{
    public float X, Y, Z, W;

    public void EndianSwap()
    {
        X = X.EndianSwap();
        Y = Y.EndianSwap();
        Z = Z.EndianSwap();
        W = W.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}