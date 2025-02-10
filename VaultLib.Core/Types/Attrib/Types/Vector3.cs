using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Types;

public struct Vector3 : IComplexType
{
    public float X, Y, Z;

    public void EndianSwap()
    {
        X = X.EndianSwap();
        Y = Y.EndianSwap();
        Z = Z.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}