using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib;

public class VinylTransform : VltBaseType
{
    public short TranslationX { get; set; }
    public short TranslationY { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        TranslationX = br.ReadInt16();
        TranslationY = br.ReadInt16();
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        bw.Write(TranslationX);
        bw.Write(TranslationY);
    }
}