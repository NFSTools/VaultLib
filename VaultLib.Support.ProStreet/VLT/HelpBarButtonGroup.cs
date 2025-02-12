using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(HelpBarButtonGroup))]
public class HelpBarButtonGroup : VltBaseType<Core.DataInterfaces.Key32>
{
    public BinKey32 TextureHash { get; set; }
    public BinKey32 LanguageHash { get; set; }
    public float TextSizeX { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        TextureHash = BinKey32.Read(br);
        LanguageHash = BinKey32.Read(br);
        TextSizeX = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        TextureHash.Write(bw);
        LanguageHash.Write(bw);
        bw.Write(TextSizeX);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}