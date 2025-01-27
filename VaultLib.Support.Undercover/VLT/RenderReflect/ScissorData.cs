using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.RenderReflect;

public class ScissorData: VltBaseType<uint>
{
    public uint X { get; set; }
    public uint Y { get; set; }
    public uint Width { get; set; }
    public uint Height { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        X = br.ReadUInt32();
        Y = br.ReadUInt32();
        Width = br.ReadUInt32();
        Height = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(X);
        bw.Write(Y);
        bw.Write(Width);
        bw.Write(Height);
    }
}