using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(CarPaintSwatch))]
public class CarPaintSwatch: VltBaseType<Core.DataInterfaces.Key32>
{
    public uint RGB { get; set; }
    public ePaintMaterialIndex MaterialA { get; set; }
    public ePaintMaterialIndex MaterialB { get; set; }
    public float Blend { get; set; }
    public ePaintSpeechColour SpeechColour { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        RGB = br.ReadUInt32();
        MaterialA = br.ReadEnum<ePaintMaterialIndex>();
        MaterialB = br.ReadEnum<ePaintMaterialIndex>();
        Blend = br.ReadSingle();
        SpeechColour = br.ReadEnum<ePaintSpeechColour>();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(RGB);
        bw.WriteEnum(MaterialA);
        bw.WriteEnum(MaterialB);
        bw.Write(Blend);
        bw.WriteEnum(SpeechColour);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}