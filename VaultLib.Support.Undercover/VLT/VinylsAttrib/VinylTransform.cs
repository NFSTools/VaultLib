using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

public class VinylTransform: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public short TranslationX { get; set; }
    public short TranslationY { get; set; }
    public byte Rotation { get; set; }
    public byte ScaleX { get; set; }
    public byte ScaleY { get; set; }
    public bool ProportionalScale { get; set; }
    public byte Shear { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        TranslationX = br.ReadInt16();
        TranslationY = br.ReadInt16();
        Rotation = br.ReadByte();
        ScaleX = br.ReadByte();
        ScaleY = br.ReadByte();
        ProportionalScale = br.ReadBoolean();
        Shear = br.ReadByte();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(TranslationX);
        bw.Write(TranslationY);
        bw.Write(Rotation);
        bw.Write(ScaleX);
        bw.Write(ScaleY);
        bw.Write(ProportionalScale);
        bw.Write(Shear);
    }
}