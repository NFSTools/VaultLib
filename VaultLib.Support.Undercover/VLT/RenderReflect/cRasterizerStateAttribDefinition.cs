// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:12 AM.

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
    [VLTTypeInfo("RenderReflect::cRasterizerStateAttribDefinition")]
    public class cRasterizerStateAttribDefinition : VLTBaseType, IReferencesStrings
    {
        public string DebugName { get; set; }
        public State_RasterizerCullMode CullMode { get; set; }
        public float DepthBias { get; set; }
        public float ScaleDepthBias { get; set; }
        public bool ScissorTestEnable { get; set; }
        public bool PrimitiveResetEnable { get; set; }
        public uint PrimitiveResetIndex { get; set; }
        public ScissorData ScissorData { get; set; }
        public State_RasterizerFillMode FillMode { get; set; }
        public bool MultiSampleAntialiasEnable { get; set; }
        public uint MultiSampleMask { get; set; }
        public bool ViewPortEnable { get; set; }
        public bool HalfPixelOffsetEnable { get; set; }
        public State_RasterizerShadeMode ShadeMode { get; set; }
        public State_RasterizerFrontFace FrontFace { get; set; }

        private Text _debugNameText;

        public override void Read(Vault vault, BinaryReader br)
        {
            _debugNameText.Read(vault, br);
            CullMode = br.ReadEnum<State_RasterizerCullMode>();
            DepthBias = br.ReadSingle();
            ScaleDepthBias = br.ReadSingle();
            ScissorTestEnable = br.ReadBoolean();
            PrimitiveResetEnable = br.ReadBoolean();
            br.AlignReader(4);
            PrimitiveResetIndex = br.ReadUInt32();
            ScissorData.Read(vault, br);
            FillMode = br.ReadEnum<State_RasterizerFillMode>();
            MultiSampleAntialiasEnable = br.ReadBoolean();
            br.AlignReader(4);
            MultiSampleMask = br.ReadUInt32();
            ViewPortEnable = br.ReadBoolean();
            HalfPixelOffsetEnable = br.ReadBoolean();
            br.AlignReader(4);
            ShadeMode = br.ReadEnum<State_RasterizerShadeMode>();
            FrontFace = br.ReadEnum<State_RasterizerFrontFace>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _debugNameText.Value = DebugName;
            _debugNameText.Write(vault, bw);
            bw.WriteEnum(CullMode);
            bw.Write(DepthBias);
            bw.Write(ScaleDepthBias);
            bw.Write(ScissorTestEnable);
            bw.Write(PrimitiveResetEnable);
            bw.AlignWriter(4);
            bw.Write(PrimitiveResetIndex);
            ScissorData.Write(vault, bw);
            bw.WriteEnum(FillMode);
            bw.Write(MultiSampleAntialiasEnable);
            bw.AlignWriter(4);
            bw.Write(MultiSampleMask);
            bw.Write(ViewPortEnable);
            bw.Write(HalfPixelOffsetEnable);
            bw.AlignWriter(4);
            bw.WriteEnum(ShadeMode);
            bw.WriteEnum(FrontFace);
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

        public cRasterizerStateAttribDefinition(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            ScissorData = new ScissorData(Class, Field, Collection);
            _debugNameText = new Text(Class, Field, Collection);
        }
    }
}