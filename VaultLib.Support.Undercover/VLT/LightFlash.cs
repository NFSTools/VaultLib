using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

// TODO: figure this out
[VltTypeInfo(nameof(LightFlash))]
public class LightFlash: VltBaseType<uint>
{
    public float Value1 { get; set; }
    public float Value2 { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Value1 = br.ReadSingle();
        Value2 = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Value1);
        bw.Write(Value2);
    }
}