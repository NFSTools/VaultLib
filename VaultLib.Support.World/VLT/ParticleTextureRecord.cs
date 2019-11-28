using System;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(ParticleTextureRecord))]
    public class ParticleTextureRecord : VLTBaseType
    {
        public enum eTEG_ParticleTextures : uint
        {
            FX_GRASS01_BLEND_ANIM = 0xFBBC7865,
            FXONFETTI2 = 0xEC70A5AA,
            FX_GRAVEL_BLEND_ANIM = 0x7884D1C5,
            FX_SMK01_BLEND = 0x82A192AC,
            FX_SMK06_BLEND = 0x384C271,
            FX_SMOKESHADER = 0x277A2772,
            FX_GLOW02_ADDITIVE = 0x85817260,
            FX_SMK07_BLEND = 0x507EFF32,
            FX_DEBRIS_GREY = 0xF7BEC12B,
            FXACTUS_BLEND_ANIM = 0x7A3C53C7,
            FX_BUZZARD_01 = 0x3E86ED5E,
            FX_DEBRIS01_BLEND_ANIM = 0x7357B3E,
            FX_SPARK05_ADDITIVE = 0xD5873F0B,
            FX_SMK04_BLEND = 0x699048EF,
            FX_SMK05_BLEND = 0xB68A85B0,
            FX_DEBRISSTONE1_BLEND = 0x1647BD33,
            FX_STRIPE03_ADDITIVE = 0x935611BF,
            FX_SMK02_BLEND = 0xCF9BCF6D,
            FX_SPARK04_ADDITIVE = 0xD17CADEA,
            FX_SMK03_BLEND = 0x1C960C2E,
            FX_STRIPE01_ADDITIVE = 0x8B40EF7D,
            FXOOD_PLANKS_BLEND_ANIMOLR = 0x773FE834,
            FX_GLASS01_BLEND_ANIM = 0x8AA12C5F,
            FX_GLOW01_ADDITIVE = 0x8176E13F,
            FX_SPARK02_ADDITIVE = 0xC9678BA8,
            FX_HAZE1 = 0x7E5ECB75,
            FX_DEBRIS02_BLEND = 0x9E6479BB,
            FX_SMOKESHADER2 = 0x16BF15E4,
            FX_STRIPE01_BLEND = 0x8D2E6C98,
            FXOOD_PLANKS_DRK_BLEND_ANIMOLR = 0x945CF2D4,
            FX_SPARK01_ADDITIVE = 0xC55CFA87,
            FXATER02_BLEND = 0xA8F05A25,
            FX_BEAN_BLEND = 0x8D746D76,
            FX_HAZE2 = 0x7E5ECB76,
            FX_SPARK01_ADDITIVE_ANIM = 0xEA4DB8EB,
            FX_FLARE_01_ADDITIVE = 0x81AD168F,
            FX_SMK04_ADDITIVE = 0x90D50074,
            FXOOD_PLANKS_BLEND_ANIM = 0xA665E9A5,
            FX_STRIPE02_ADDITIVE = 0x8F4B809E,
            FXATER01_BLEND = 0x5BF61D64,
            FXATER03_BLEND = 0xF5EA96E6,
            FXONFETTI = 0x35B5D678,
            FX_GREYGRAD32 = 0x54E298F6,
            FX_SPARK03_ADDITIVE = 0xCD721CC9,
            FX_GREYGRAD32_ADD = 0xC607597E,
        };

        public eTEG_ParticleTextures mEnum { get; set; }
        public uint mIndex { get; set; }

        /*  eTEG_ParticleTextures mEnum;
  unsigned int mIndex;*/
        public override void Read(Vault vault, BinaryReader br)
        {
            mEnum = br.ReadEnum<eTEG_ParticleTextures>();
            mIndex = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(mEnum);
            bw.Write(mIndex);
        }
    }
}