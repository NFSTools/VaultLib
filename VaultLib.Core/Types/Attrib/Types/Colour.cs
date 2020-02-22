using System.IO;
using VaultLib.Core.Data;

namespace VaultLib.Core.Types.Attrib.Types
{
    [VLTTypeInfo("Attrib::Types::Colour")]
    public class Colour : VLTBaseType
    {
        public Colour(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public Colour(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            uint value = br.ReadUInt32();

            R = (byte) ((value >> 24) & 0xff);
            G = (byte) ((value >> 16) & 0xff);
            B = (byte) ((value >> 8) & 0xff);
            A = (byte) (value & 0xff);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((R << 24) | (G << 16) | (B << 8) | (A & 0xFF));
        }
    }
}