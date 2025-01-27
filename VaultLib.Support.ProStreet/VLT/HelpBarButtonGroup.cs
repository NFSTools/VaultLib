using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(HelpBarButtonGroup))]
public class HelpBarButtonGroup: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public uint TextureHash { get; set; }
    public uint LanguageHash { get; set; }
    public float TextSizeX { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        TextureHash = br.ReadUInt32();
        LanguageHash = br.ReadUInt32();
        TextSizeX = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(TextureHash);
        bw.Write(LanguageHash);
        bw.Write(TextSizeX);
    }
}