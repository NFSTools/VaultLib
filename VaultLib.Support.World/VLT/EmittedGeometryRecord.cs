// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 10:42 AM.

using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(EmittedGeometryRecord))]
    public class EmittedGeometryRecord : VLTBaseType
    {
        public enum EmittedGeo : uint
        {
            EmittedGeo_NONE = 0x0,
            EmittedGeo_DEBRIS_CHUNKY_01 = 0x4C827F49,
            EmittedGeo_DEBRIS_CONCRETE_01 = 0x28A3EDEA,
            EmittedGeo_DEBRIS_CONCRETE_02 = 0x28A3EDEB,
            EmittedGeo_DEBRIS_CONCRETE_03 = 0x28A3EDEC,
            EmittedGeo_DEBRIS_LEAF_01 = 0xEC9DCA4F,
            EmittedGeo_DEBRIS_PLANK_01 = 0x4CBCA56D,
            EmittedGeo_DEBRIS_PLANK_02 = 0x4CBCA56E,
            EmittedGeo_DEBRIS_ROCKS_BUNCH_01 = 0x7ADC5A88,
            EmittedGeo_DEBRIS_ROCK_01 = 0xD1F5BC06,
            EmittedGeo_DEBRIS_STICK_01 = 0x8B364195,
            EmittedGeo_FX_CARDEBRIS_01 = 0x9F38528B,
            EmittedGeo_FX_CARDEBRIS_02 = 0x9F38528C,
            EmittedGeo_FX_CARDEBRIS_03 = 0x9F38528D,
            EmittedGeo_FX_TESTDEBRIS_01 = 0xEA4E6FF5,
            EmittedGeo_FX_TESTDEBRIS_02 = 0xEA4E6FF6,
            EmittedGeo_FX_TESTDEBRIS_03 = 0xEA4E6FF7,
            EmittedGeo_PLANE01 = 0xD7FD6170,
            EmittedGeo_XOU_DOLLARICON_1A_00 = 0xCB4C8691,
            EmittedGeo_XP_SMOKEBALL_CURL = 0xE5AD4535,
            EmittedGeo_XP_SMOKEBALL_PUFF = 0xE5B46490,
            EmittedGeo_XP_SMOKEWII_PUFF01 = 0x642446BF,
            EmittedGeo_DEBRIS_SAUSAGE_01 = 0x17C199C0,
            EmittedGeo_FX_MARKER_RING_YELLOW = 0x01C76D88
            //EmittedGeo_NUM = 0x15,
        };

        public EmittedGeo Value { get; set; }
        public uint Index { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Value = br.ReadEnum<EmittedGeo>();
            Index = br.ReadUInt32();

            //if (!Enum.IsDefined(typeof(EmittedGeo), Value))
            //{
            //    Debug.WriteLine("undefined EmittedGeo: {0:X8}", (uint) Value);
            //}
            //Debug.Assert(Enum.IsDefined(typeof(EmittedGeo), Value));
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(Value);
            bw.Write(Index);
        }
    }
}