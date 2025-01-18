// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/20/2019 @ 11:12 AM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VltTypeInfo("RenderReflect::cRasterizerStateAttribDefinition")]
    public class cRasterizerStateAttribDefinition : VltBaseType, IReferencesStrings
    {
        public string DebugName { get; set; } = string.Empty;
        public State_RasterizerCullMode CullMode { get; set; }
        public float DepthBias { get; set; }
        public float ScaleDepthBias { get; set; }
        public bool ScissorTestEnable { get; set; }
        public bool PrimitiveResetEnable { get; set; }
        public uint PrimitiveResetIndex { get; set; }
        public ScissorData ScissorData { get; set; } = new();
        public State_RasterizerFillMode FillMode { get; set; }
        public bool MultiSampleAntialiasEnable { get; set; }
        public uint MultiSampleMask { get; set; }
        public bool ViewPortEnable { get; set; }
        public bool HalfPixelOffsetEnable { get; set; }
        public State_RasterizerShadeMode ShadeMode { get; set; }
        public State_RasterizerFrontFace FrontFace { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            DebugName = context.ReadString(br);
            CullMode = br.ReadEnum<State_RasterizerCullMode>();
            DepthBias = br.ReadSingle();
            ScaleDepthBias = br.ReadSingle();
            ScissorTestEnable = br.ReadBoolean();
            PrimitiveResetEnable = br.ReadBoolean();
            br.AlignReader(4);
            PrimitiveResetIndex = br.ReadUInt32();
            ScissorData.Read(context, fieldContext, br);
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

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            context.WriteString(DebugName, fieldContext, bw);
            bw.WriteEnum(CullMode);
            bw.Write(DepthBias);
            bw.Write(ScaleDepthBias);
            bw.Write(ScissorTestEnable);
            bw.Write(PrimitiveResetEnable);
            bw.AlignWriter(4);
            bw.Write(PrimitiveResetIndex);
            ScissorData.Write(context, fieldContext, bw);
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

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
        }

        public IEnumerable<string> GetStrings()
        {
            return new[] { DebugName };
        }
    }
}