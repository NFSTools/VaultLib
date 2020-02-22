using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.VinylsAttrib
{
    public class VinylTransform : VLTBaseType
    {
        public VinylTransform(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public VinylTransform(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public short TranslationX { get; set; }
        public short TranslationY { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            TranslationX = br.ReadInt16();
            TranslationY = br.ReadInt16();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(TranslationX);
            bw.Write(TranslationY);
        }
    }
}