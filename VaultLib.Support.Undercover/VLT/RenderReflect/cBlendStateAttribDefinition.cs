// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 11:10 PM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.RenderReflect;

[VltTypeInfo("RenderReflect::cBlendStateAttribDefinition")]
public class cBlendStateAttribDefinition: VltBaseType<VaultLib.Core.DataInterfaces.Key32>, IReferencesStrings<VaultLib.Core.DataInterfaces.Key32>
{
    public string DebugName { get; set; } = string.Empty;

    public bool BlendEnable { get; set; }
    public bool AlphaTestEnable { get; set; }
    public uint AlphaTestRef { get; set; }
    public State_BlendFunc AlphaTestFunc { get; set; }
    public State_BlendInput[] SourceColor { get; set; } = new State_BlendInput[4];
    public State_BlendInput[] DestColor { get; set; } = new State_BlendInput[4];
    public State_BlendOp[] OperationColor { get; set; } = new State_BlendOp[4];
    public State_BlendInput[] SourceAlpha { get; set; } = new State_BlendInput[4];
    public State_BlendInput[] DestAlpha { get; set; } = new State_BlendInput[4];
    public State_BlendOp[] OperationAlpha { get; set; } = new State_BlendOp[4];
    public Vector4 BlendFactor { get; set; }
    public bool[] RGBAEnableRT0 { get; set; } = new bool[4];
    public bool[] RGBAEnableRT1 { get; set; } = new bool[4];
    public bool[] RGBAEnableRT2 { get; set; } = new bool[4];
    public bool[] RGBAEnableRT3 { get; set; } = new bool[4];
    public bool AlphaToMaskEnable_XENON { get; set; }
    public bool[] HiPrecisionBlendEnable_XENON { get; set; } = new bool[4];
    public bool[] BlendEnable_PS3 { get; set; } = new bool[4];
    public bool BlendFactorF16_PS3 { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        DebugName = context.ReadString(br);
        BlendEnable = br.ReadBoolean();
        AlphaTestEnable = br.ReadBoolean();
        br.SafeAlignReader(4);
        AlphaTestRef = br.ReadUInt32();
        AlphaTestFunc = br.ReadEnum<State_BlendFunc>();
        SourceColor = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
        DestColor = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
        OperationColor = br.ReadArray(br.ReadEnum<State_BlendOp>, 4);
        SourceAlpha = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
        DestAlpha = br.ReadArray(br.ReadEnum<State_BlendInput>, 4);
        OperationAlpha = br.ReadArray(br.ReadEnum<State_BlendOp>, 4);
        // TODO: we probably want a helper for reading structs
        BlendFactor = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        RGBAEnableRT0 = br.ReadArray(br.ReadBoolean, 4);
        RGBAEnableRT1 = br.ReadArray(br.ReadBoolean, 4);
        RGBAEnableRT2 = br.ReadArray(br.ReadBoolean, 4);
        RGBAEnableRT3 = br.ReadArray(br.ReadBoolean, 4);
        AlphaToMaskEnable_XENON = br.ReadBoolean();
        HiPrecisionBlendEnable_XENON = br.ReadArray(br.ReadBoolean, 4);
        BlendEnable_PS3 = br.ReadArray(br.ReadBoolean, 4);
        BlendFactorF16_PS3 = br.ReadBoolean();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        context.WriteString(DebugName, fieldContext, bw);
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
        // TODO: we probably want a helper for writing structs
        bw.Write(BlendFactor.X);
        bw.Write(BlendFactor.Y);
        bw.Write(BlendFactor.Z);
        bw.Write(BlendFactor.W);
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

    public void ReadPointerData(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
    }

    public void WritePointerData(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
    }

    public void AddPointers(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext)
    {
    }

    public IEnumerable<string> GetStrings()
    {
        return new[] { DebugName };
    }
}