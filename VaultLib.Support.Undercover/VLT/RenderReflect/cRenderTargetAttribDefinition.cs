using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    [VLTTypeInfo("RenderReflect::cRenderTargetAttribDefinition")]
    public class cRenderTargetAttribDefinition : VLTBaseType
    {
        public cRenderTargetAttribDefinition(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public cRenderTargetAttribDefinition(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint NumColorSurfaces { get; set; }
        public ScreenSizeMode WidthMode { get; set; }
        public uint Width { get; set; }
        public float WidthMultiplier { get; set; }
        public ScreenSizeMode HeightMode { get; set; }
        public uint Height { get; set; }
        public float HeightMultiplier { get; set; }
        public RenderTargetMode ColorTargetMode { get; set; }
        public PixelFormatType TargetColorFormat { get; set; }
        public RenderTargetMode DepthStencilTargetMode { get; set; }
        public PixelFormatType TargetDepthStencilFormat { get; set; }
        public MultiSampleMode MultiSampleMode { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            NumColorSurfaces = br.ReadUInt32();
            WidthMode = br.ReadEnum<ScreenSizeMode>();
            Width = br.ReadUInt32();
            WidthMultiplier = br.ReadSingle();
            HeightMode = br.ReadEnum<ScreenSizeMode>();
            Height = br.ReadUInt32();
            HeightMultiplier = br.ReadSingle();
            ColorTargetMode = br.ReadEnum<RenderTargetMode>();
            TargetColorFormat = br.ReadEnum<PixelFormatType>();
            DepthStencilTargetMode = br.ReadEnum<RenderTargetMode>();
            TargetDepthStencilFormat = br.ReadEnum<PixelFormatType>();
            MultiSampleMode = br.ReadEnum<MultiSampleMode>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(NumColorSurfaces);
            bw.WriteEnum(WidthMode);
            bw.Write(Width);
            bw.Write(WidthMultiplier);
            bw.WriteEnum(HeightMode);
            bw.Write(Height);
            bw.Write(HeightMultiplier);
            bw.WriteEnum(ColorTargetMode);
            bw.WriteEnum(TargetColorFormat);
            bw.WriteEnum(DepthStencilTargetMode);
            bw.WriteEnum(TargetDepthStencilFormat);
            bw.WriteEnum(MultiSampleMode);
        }
    }
}