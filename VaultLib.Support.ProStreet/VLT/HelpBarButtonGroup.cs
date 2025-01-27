using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(HelpBarButtonGroup))]
public class HelpBarButtonGroup: VltBaseType<uint>
{
    public uint TextureHash { get; set; }
    public uint LanguageHash { get; set; }
    public float TextSizeX { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        TextureHash = br.ReadUInt32();
        LanguageHash = br.ReadUInt32();
        TextSizeX = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(TextureHash);
        bw.Write(LanguageHash);
        bw.Write(TextSizeX);
    }
}