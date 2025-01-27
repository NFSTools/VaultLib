using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.DamageFilter;

[VltTypeInfo("DamageFilter::Control")]
public class Control: VltBaseType<uint>
{
    public uint Allow { get; set; }
    public uint Reject { get; set; }
    public float MaxCausalityTime { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Allow = br.ReadUInt32();
        Reject = br.ReadUInt32();
        MaxCausalityTime = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Allow);
        bw.Write(Reject);
        bw.Write(MaxCausalityTime);
    }
}