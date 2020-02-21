using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(HelpBarButtonGroup))]
    public class HelpBarButtonGroup : VLTBaseType
    {
        public HelpBarButtonGroup(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public HelpBarButtonGroup(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint TextureHash { get; set; }
        public uint LanguageHash { get; set; }
        public float TextSizeX { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            TextureHash = br.ReadUInt32();
            LanguageHash = br.ReadUInt32();
            TextSizeX = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(TextureHash);
            bw.Write(LanguageHash);
            bw.Write(TextSizeX);
        }
    }
}