// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 4:52 PM.

namespace VaultLib.Core.Types.Attrib.Types;

[VltTypeInfo("Attrib::Types::FloatColour")]
public struct FloatColour
{
    public float R;
    public float G;
    public float B;
    public float A;

    public override string ToString()
    {
        return $"R: {R} G: {G} B: {B} A: {A}";
    }
}