using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(FEVinylGroupInfo))]
public class FEVinylGroupInfo: VltBaseType<Core.DataInterfaces.Key32>
{
    public uint Value1 { get; set; }
    public uint Value2 { get; set; }
    public uint Value3 { get; set; }
    public uint Value4 { get; set; }
    public uint Value5 { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Value1 = br.ReadUInt32();
        Value2 = br.ReadUInt32();
        Value3 = br.ReadUInt32();
        Value4 = br.ReadUInt32();
        Value5 = br.ReadUInt32();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Value1);
        bw.Write(Value2);
        bw.Write(Value3);
        bw.Write(Value4);
        bw.Write(Value5);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}