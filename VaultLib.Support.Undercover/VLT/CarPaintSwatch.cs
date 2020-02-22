using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(CarPaintSwatch))]
    public class CarPaintSwatch : VLTBaseType
    {
        public CarPaintSwatch(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CarPaintSwatch(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint RGB { get; set; }
        public ePaintMaterialIndex MaterialA { get; set; }
        public ePaintMaterialIndex MaterialB { get; set; }
        public float Blend { get; set; }
        public ePaintSpeechColour SpeechColour { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            RGB = br.ReadUInt32();
            MaterialA = br.ReadEnum<ePaintMaterialIndex>();
            MaterialB = br.ReadEnum<ePaintMaterialIndex>();
            Blend = br.ReadSingle();
            SpeechColour = br.ReadEnum<ePaintSpeechColour>();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(RGB);
            bw.WriteEnum(MaterialA);
            bw.WriteEnum(MaterialB);
            bw.Write(Blend);
            bw.WriteEnum(SpeechColour);
        }
    }
}