// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 11:10 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VLTTypeInfo("RenderReflect::cBlendStateAttribDefinition")]
    public class cBlendStateAttribDefinition : VLTBaseType, IReferencesStrings
    {
        public string DebugName { get; set; }

        public bool BlendEnable { get; set; }
        public bool AlphaTestEnable { get; set; }
        public uint AlphaTestRef { get; set; }
        public State_BlendFunc AlphaTestFunc { get; set; }
        public State_BlendInput[] SourceColor { get; set; }
        public State_BlendInput[] DestColor { get; set; }
        public State_BlendOp[] OperationColor { get; set; }
        public State_BlendInput[] SourceAlpha { get; set; }
        public State_BlendInput[] DestAlpha { get; set; }
        public State_BlendOp[] OperationAlpha { get; set; }
        public Vector4 BlendFactor { get; set; }
        public bool[] RGBAEnableRT0 { get; set; }
        public bool[] RGBAEnableRT1 { get; set; }
        public bool[] RGBAEnableRT2 { get; set; }
        public bool[] RGBAEnableRT3 { get; set; }
        public bool AlphaToMaskEnable_XENON { get; set; }
        public bool[] HiPrecisionBlendEnable_XENON { get; set; }
        public bool[] BlendEnable_PS3 { get; set; }
        public bool BlendFactorF16_PS3 { get; set; }
        private Text _debugNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _debugNameText = new Text { Class = Class, Collection = Collection, Field = Field };
            _debugNameText.Read(vault, br);
            BlendEnable = br.ReadBoolean();
            AlphaTestEnable = br.ReadBoolean();
            br.AlignReader(4);
            AlphaTestRef = br.ReadUInt32();
            AlphaTestFunc = br.ReadEnum<State_BlendFunc>();
            SourceColor = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
            DestColor = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
            OperationColor = br.ReadArray(br.ReadEnum<State_BlendOp>, 4);
            SourceAlpha = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
            DestAlpha = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
            OperationAlpha = br.ReadArray(br.ReadEnum<State_BlendOp>, 4);
            BlendFactor = new Vector4();
            BlendFactor.Read(vault, br);
            RGBAEnableRT0 = br.ReadArray(br.ReadBoolean, 4);
            RGBAEnableRT1 = br.ReadArray(br.ReadBoolean, 4);
            RGBAEnableRT2 = br.ReadArray(br.ReadBoolean, 4);
            RGBAEnableRT3 = br.ReadArray(br.ReadBoolean, 4);
            AlphaToMaskEnable_XENON = br.ReadBoolean();
            HiPrecisionBlendEnable_XENON = br.ReadArray(br.ReadBoolean, 4);
            BlendEnable_PS3 = br.ReadArray(br.ReadBoolean, 4);
            BlendFactorF16_PS3 = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _debugNameText.Write(vault, bw);
            bw.Write(BlendEnable);
            bw.Write(AlphaTestEnable);
            bw.AlignWriter(4);
            bw.Write(AlphaTestRef);
            bw.WriteEnum(AlphaTestFunc);
            bw.WriteArray(SourceColor, bw.WriteEnum);
            bw.WriteArray(DestColor, bw.WriteEnum);
            bw.WriteArray(OperationColor, bw.WriteEnum);
            bw.WriteArray(SourceAlpha, bw.WriteEnum);
            bw.WriteArray(DestAlpha, bw.WriteEnum);
            bw.WriteArray(OperationAlpha, bw.WriteEnum);
            BlendFactor.Write(vault, bw);
            bw.WriteArray(RGBAEnableRT0, bw.Write);
            bw.WriteArray(RGBAEnableRT1, bw.Write);
            bw.WriteArray(RGBAEnableRT2, bw.Write);
            bw.WriteArray(RGBAEnableRT3, bw.Write);
            bw.Write(AlphaToMaskEnable_XENON);
            bw.WriteArray(HiPrecisionBlendEnable_XENON, bw.Write);
            bw.WriteArray(BlendEnable_PS3, bw.Write);
            bw.Write(BlendFactorF16_PS3);
            bw.AlignWriter(4);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            _debugNameText.ReadPointerData(vault, br);
            DebugName = _debugNameText.Value;
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _debugNameText.WritePointerData(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            _debugNameText.AddPointers(vault);
        }

        public IEnumerable<string> GetStrings()
        {
            return _debugNameText.GetStrings();
        }
    }
}