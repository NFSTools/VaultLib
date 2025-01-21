using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo(nameof(VinylColor))]
public class VinylColor : VltBaseType
{
    public sbyte Swatch { get; set; }
    public sbyte Saturation { get; set; }
    public sbyte Brightness { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        Swatch = br.ReadSByte();
        Saturation = br.ReadSByte();
        Brightness = br.ReadSByte();
        br.AlignReader(4);
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.Write(Swatch);
        bw.Write(Saturation);
        bw.Write(Brightness);
        bw.AlignWriter(4);
    }
}