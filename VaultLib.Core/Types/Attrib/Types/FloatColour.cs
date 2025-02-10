// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:52 PM.

using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Types;

[VltTypeInfo("Attrib::Types::FloatColour")]
public struct FloatColour : IComplexType
{
    public float R;
    public float G;
    public float B;
    public float A;

    public override string ToString()
    {
        return $"R: {R} G: {G} B: {B} A: {A}";
    }

    public void EndianSwap()
    {
        BinaryExtensions.EndianSwap(ref R);
        BinaryExtensions.EndianSwap(ref G);
        BinaryExtensions.EndianSwap(ref B);
        BinaryExtensions.EndianSwap(ref A);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}