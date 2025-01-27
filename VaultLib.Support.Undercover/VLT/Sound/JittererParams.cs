using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Sound;

[VltTypeInfo("Sound::JittererParams")]
// TODO: determine what this is
public class JittererParams: VltBaseType<uint>
{
    public float Unknown1 { get; set; }
    public float Unknown2 { get; set; }
    public float Unknown3 { get; set; }
    public float Unknown4 { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Unknown1 = br.ReadSingle();
        Unknown2 = br.ReadSingle();
        Unknown3 = br.ReadSingle();
        Unknown4 = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Unknown1);
        bw.Write(Unknown2);
        bw.Write(Unknown3);
        bw.Write(Unknown4);
    }
}