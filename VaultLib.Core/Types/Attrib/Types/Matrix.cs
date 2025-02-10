using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Types;

public struct Matrix : IComplexType
{
    public float M11;
    public float M12;
    public float M13;
    public float M14;
    public float M21;
    public float M22;
    public float M23;
    public float M24;
    public float M31;
    public float M32;
    public float M33;
    public float M34;
    public float M41;
    public float M42;
    public float M43;
    public float M44;

    public void EndianSwap()
    {
        M11 = M11.EndianSwap();
        M12 = M12.EndianSwap();
        M13 = M13.EndianSwap();
        M14 = M14.EndianSwap();
        M21 = M21.EndianSwap();
        M22 = M22.EndianSwap();
        M23 = M23.EndianSwap();
        M24 = M24.EndianSwap();
        M31 = M31.EndianSwap();
        M32 = M32.EndianSwap();
        M33 = M33.EndianSwap();
        M34 = M34.EndianSwap();
        M41 = M41.EndianSwap();
        M42 = M42.EndianSwap();
        M43 = M43.EndianSwap();
        M44 = M44.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}