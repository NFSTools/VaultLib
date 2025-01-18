using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    public class ScissorData : VltBaseType
    {
        public uint X { get; set; }
        public uint Y { get; set; }
        public uint Width { get; set; }
        public uint Height { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            X = br.ReadUInt32();
            Y = br.ReadUInt32();
            Width = br.ReadUInt32();
            Height = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Width);
            bw.Write(Height);
        }
    }
}