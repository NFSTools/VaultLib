using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Sound;

[VltTypeInfo("Sound::JittererParams")]
// TODO: determine what this is
public class JittererParams: VltBaseType<Core.DataInterfaces.Key32>
{
    public float Unknown1 { get; set; }
    public float Unknown2 { get; set; }
    public float Unknown3 { get; set; }
    public float Unknown4 { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Unknown1 = br.ReadSingle();
        Unknown2 = br.ReadSingle();
        Unknown3 = br.ReadSingle();
        Unknown4 = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Unknown1);
        bw.Write(Unknown2);
        bw.Write(Unknown3);
        bw.Write(Unknown4);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}