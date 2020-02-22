using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib
{
    [VLTTypeInfo(nameof(VinylColor))]
    public class VinylColor : VLTBaseType
    {
        public VinylColor(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public VinylColor(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public sbyte Swatch { get; set; }
        public sbyte Saturation { get; set; }
        public sbyte Brightness { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Swatch = br.ReadSByte();
            Saturation = br.ReadSByte();
            Brightness = br.ReadSByte();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Swatch);
            bw.Write(Saturation);
            bw.Write(Brightness);
            bw.AlignWriter(4);
        }
    }
}