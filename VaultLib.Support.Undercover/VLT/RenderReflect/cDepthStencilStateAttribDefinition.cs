// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 11:56 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VLTTypeInfo("RenderReflect::cDepthStencilStateAttribDefinition")]
    public class cDepthStencilStateAttribDefinition : VLTBaseType, IReferencesStrings
    {
        public string DebugName { get; set; }

        public bool TwoSidedStencilMode { get; set; }
        public bool ZEnable { get; set; }
        public bool ZWriteEnable { get; set; }
        public bool StencilEnable { get; set; }
        public State_GPUFunc ZFunc { get; set; }
        public State_GPUFunc StencilFunc { get; set; }
        public State_GPUFunc StencilFuncCCW { get; set; }
        public uint StencilRef { get; set; }
        public uint StencilRefCCW { get; set; }
        public uint StencilMask { get; set; }
        public uint StencilMaskCCW { get; set; }
        public uint StencilWriteMask { get; set; }
        public uint StencilWriteMaskCCW { get; set; }
        public State_StencilOp StencilFailOp { get; set; }
        public State_StencilOp StencilFailOpCCW { get; set; }
        public State_StencilOp StencilZFailOp { get; set; }
        public State_StencilOp StencilZFailOpCCW { get; set; }
        public State_StencilOp StencilPassOp { get; set; }
        public State_StencilOp StencilPassOpCCW { get; set; }
        public State_HiCompare HiStencilFunc { get; set; }
        public uint HiStencilRef { get; set; }
        public bool HiStencilEnable { get; set; }
        public bool HiStencilWriteEnable { get; set; }

        private Text _debugNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _debugNameText = new Text(Class, Field, Collection);
            _debugNameText.Read(vault, br);

            TwoSidedStencilMode = br.ReadBoolean();
            ZEnable = br.ReadBoolean();
            ZWriteEnable = br.ReadBoolean();
            StencilEnable = br.ReadBoolean();
            ZFunc = br.ReadEnum<State_GPUFunc>();
            StencilFunc = br.ReadEnum<State_GPUFunc>();
            StencilFuncCCW = br.ReadEnum<State_GPUFunc>();
            StencilRef = br.ReadUInt32();
            StencilRefCCW = br.ReadUInt32();
            StencilMask = br.ReadUInt32();
            StencilMaskCCW = br.ReadUInt32();
            StencilWriteMask = br.ReadUInt32();
            StencilWriteMaskCCW = br.ReadUInt32();
            StencilFailOp = br.ReadEnum<State_StencilOp>();
            StencilFailOpCCW = br.ReadEnum<State_StencilOp>();
            StencilZFailOp = br.ReadEnum<State_StencilOp>();
            StencilZFailOpCCW = br.ReadEnum<State_StencilOp>();
            StencilPassOp = br.ReadEnum<State_StencilOp>();
            StencilPassOpCCW = br.ReadEnum<State_StencilOp>();
            HiStencilFunc = br.ReadEnum<State_HiCompare>();
            HiStencilRef = br.ReadUInt32();
            HiStencilEnable = br.ReadBoolean();
            HiStencilWriteEnable = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _debugNameText.Write(vault, bw);
            bw.Write(TwoSidedStencilMode);
            bw.Write(ZEnable);
            bw.Write(ZWriteEnable);
            bw.Write(StencilEnable);
            bw.WriteEnum(ZFunc);
            bw.WriteEnum(StencilFunc);
            bw.WriteEnum(StencilFuncCCW);
            bw.Write(StencilRef);
            bw.Write(StencilRefCCW);
            bw.Write(StencilMask);
            bw.Write(StencilMaskCCW);
            bw.Write(StencilWriteMask);
            bw.Write(StencilWriteMaskCCW);
            bw.WriteEnum(StencilFailOp);
            bw.WriteEnum(StencilFailOpCCW);
            bw.WriteEnum(StencilZFailOp);
            bw.WriteEnum(StencilZFailOpCCW);
            bw.WriteEnum(StencilPassOp);
            bw.WriteEnum(StencilPassOpCCW);
            bw.WriteEnum(HiStencilFunc);
            bw.Write(HiStencilRef);
            bw.Write(HiStencilEnable);
            bw.Write(HiStencilWriteEnable);
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

        public cDepthStencilStateAttribDefinition(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public cDepthStencilStateAttribDefinition(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}