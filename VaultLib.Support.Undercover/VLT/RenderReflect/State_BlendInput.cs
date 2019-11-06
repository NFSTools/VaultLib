// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 11:14 PM.

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    public enum State_BlendInput
    {
        kStateBlendInput_Zero = 0x0,
        kStateBlendInput_One = 0x1,
        kStateBlendInput_SrcColour = 0x2,
        kStateBlendInput_InvSrcColour = 0x3,
        kStateBlendInput_SrcAlpha = 0x4,
        kStateBlendInput_InvSrcAlpha = 0x5,
        kStateBlendInput_DestColour = 0x6,
        kStateBlendInput_InvDestColour = 0x7,
        kStateBlendInput_DestAlpha = 0x8,
        kStateBlendInput_InvDestAlpha = 0x9,
        kStateBlendInput_ConstantAlpha = 0xA,
        kStateBlendInput_InvConstantAlpha = 0xB,
        kStateBlendInput_SrcAlphaSaturate = 0xC,
        kStateBlendInput_BlendFactor_XENON = 0xD,
        kStateBlendInput_InvBlendFactor_XENON = 0xE,
        kStateBlendInput_ConstantColour_PS3 = 0xF,
        kStateBlendInput_InvConstantColour_PS3 = 0x10,
    }
}