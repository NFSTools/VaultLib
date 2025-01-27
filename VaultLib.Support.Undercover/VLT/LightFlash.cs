using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

// TODO: figure this out
[VltTypeInfo(nameof(LightFlash))]
public class LightFlash: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public float Value1 { get; set; }
    public float Value2 { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Value1 = br.ReadSingle();
        Value2 = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Value1);
        bw.Write(Value2);
    }
}