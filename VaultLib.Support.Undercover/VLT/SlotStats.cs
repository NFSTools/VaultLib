// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:25 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(SlotStats))]
    public class SlotStats : VLTBaseType, IReferencesStrings
    {
        public enum StatsModeFlag
        {
            STATSMODE_GRIP = 0x1,
            STATSMODE_DRIFT = 0x2,
            STATSMODE_DRAG = 0x4,
            STATSMODE_SPEED = 0x8,
            STATSMODE_ALL = 0xF,
        }

        public enum FEPhysicsStatType
        {
            NO_STAT = 0x0,
            TIME_0_TO_100_MPH = 0x1,
            TIME_0_TO_60_MPH = 0x2,
            TIME_70_TO_150_MPH = 0x3,
            TIME_100_TO_0_MPH = 0x4,
            TIME_60_TO_0_MPH = 0x5,
            DISTANCE_60_TO_0_MPH = 0x6,
            DISTANCE_100_TO_0_MPH = 0x7,
            TIME_QUARTER_MILE = 0x8,
            SPEED_QUARTER_MILE_MPH = 0x9,
            TIME_60_FEET = 0xA,
            TOPSPEED = 0xB,
            MAX_GS_30_MPH = 0xC,
            MAX_GS_60_MPH = 0xD,
            MAX_GS_100_MPH = 0xE,
            TIME_CORNER_200M_40M_300M = 0xF,
            DOWNFORCE = 0x10,
            DRAG = 0x11,
            FRONT_TOE_30_MPH = 0x12,
            FRONT_TOE_60_MPH = 0x13,
            FRONT_TOE_100_MPH = 0x14,
            CASTER = 0x15,
            FRONT_WEIGHT_BIAS = 0x16,
            EBRAKE = 0x17,
            STEERING_ALIGNMENT_TORQUE = 0x18,
            AERO_CG = 0x19,
            ROLL_CENTER = 0x1A,
            SPRING_STIFFNESS = 0x1B,
            SWAYBAR_STIFFNESS = 0x1C,
            SHOCK_DIGRESSION = 0x1D,
            SHOCK_VALVING = 0x1E,
            HORSEPOWER = 0x1F,
            HORSEPOWERRPM = 0x20,
            PEAKTORQUE = 0x21,
            PEAKTORQUERPM = 0x22,
            WEIGHT = 0x23,
            WEIGHT_TO_POWER_RATIO = 0x24,
        }

        public string SlotName { get; set; }
        public StatsModeFlag ModeFlags { get; set; }
        public uint SlotDesc { get; set; }
        public uint TuningSliderListString { get; set; }
        public FEPhysicsStatType[] Stats { get; set; }

        private Text _slotNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _slotNameText = new Text(Class, Field, Collection);
            _slotNameText.Read(vault, br);
            ModeFlags = br.ReadEnum<StatsModeFlag>();
            SlotDesc = br.ReadUInt32();
            TuningSliderListString = br.ReadUInt32();
            Stats = br.ReadArray(br.ReadEnum<FEPhysicsStatType>, 2);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _slotNameText.Write(vault, bw);
            bw.WriteEnum(ModeFlags);
            bw.Write(SlotDesc);
            bw.Write(TuningSliderListString);
            bw.WriteArray(Stats, bw.WriteEnum);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _slotNameText.ReadPointerData(vault, br);
            SlotName = _slotNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _slotNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _slotNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _slotNameText.GetStrings();
        }

        public SlotStats(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public SlotStats(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}