using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo(nameof(VinylColor))]
public class VinylColor: VltBaseType<uint>
{
    public byte Swatch { get; set; }
    public byte Saturation { get; set; }
    public byte Brightness { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Swatch = br.ReadByte();
        Saturation = br.ReadByte();
        Brightness = br.ReadByte();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Swatch);
        bw.Write(Saturation);
        bw.Write(Brightness);
    }
}