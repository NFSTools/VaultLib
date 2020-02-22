using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    public class ScissorData : VLTBaseType
    {
        public ScissorData(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public ScissorData(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint X { get; set; }
        public uint Y { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            X = br.ReadUInt32();
            Y = br.ReadUInt32();
            Width = br.ReadUInt32();
            Height = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Width);
            bw.Write(Height);
        }
    }
}