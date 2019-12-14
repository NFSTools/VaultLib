// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:54 AM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(ParticleAnimationInfo))]
    public class ParticleAnimationInfo : VLTBaseType
    {
        public enum EffectParticleAnimation
        {
            ANIMATE_PARTICLE_NONE = 0x0,
            ANIMATE_PARTICLE_2x2 = 0x2,
            ANIMATE_PARTICLE_4x4 = 0x4,
            ANIMATE_PARTICLE_8x8 = 0x8,
            ANIMATE_PARTICLE_16x16 = 0x10,
        };

        public EffectParticleAnimation AnimType { get; set; }
        public byte FPS { get; set; }
        public bool RandomStartFrame { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            AnimType = br.ReadEnum<EffectParticleAnimation>();
            FPS = br.ReadByte();
            RandomStartFrame = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(AnimType);
            bw.Write(FPS);
            bw.Write(RandomStartFrame);
            bw.AlignWriter(4);
        }

        public ParticleAnimationInfo(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public ParticleAnimationInfo(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}