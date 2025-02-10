// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:54 AM.

using System.Buffers.Binary;
using System.Runtime.InteropServices;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(ParticleAnimationInfo))]
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
public struct ParticleAnimationInfo : IComplexType
{
    public enum EffectParticleAnimation
    {
        ANIMATE_PARTICLE_NONE = 0x0,
        ANIMATE_PARTICLE_2x2 = 0x2,
        ANIMATE_PARTICLE_4x4 = 0x4,
        ANIMATE_PARTICLE_8x8 = 0x8,
        ANIMATE_PARTICLE_16x16 = 0x10,
    };

    [FieldOffset(0)] public EffectParticleAnimation AnimType;
    [FieldOffset(4)] public byte FPS;

    [FieldOffset(5)] [MarshalAs(UnmanagedType.U1)]
    public bool RandomStartFrame;

    public void EndianSwap()
    {
        AnimType = (EffectParticleAnimation)BinaryPrimitives.ReverseEndianness((uint)AnimType);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}