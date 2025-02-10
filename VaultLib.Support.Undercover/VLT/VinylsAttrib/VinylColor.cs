using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

[VltTypeInfo(nameof(VinylColor))]
public class VinylColor : VltBaseType<Core.DataInterfaces.Key32>
{
    public byte Swatch { get; set; }
    public byte Saturation { get; set; }
    public byte Brightness { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Swatch = br.ReadByte();
        Saturation = br.ReadByte();
        Brightness = br.ReadByte();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Swatch);
        bw.Write(Saturation);
        bw.Write(Brightness);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}