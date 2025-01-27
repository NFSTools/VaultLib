using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CameraReactionRecord))]
public class CameraReactionRecord: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public CameraReactionType Type { get; set; }
    public float InputMin { get; set; }
    public float[] ValueMin { get; set; } = new float[2];
    public float InputMax { get; set; }
    public float[] ValueMax { get; set; } = new float[2];

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Type = br.ReadEnum<CameraReactionType>();
        InputMin = br.ReadSingle();
        ValueMin = br.ReadArray(br.ReadSingle, 2);
        InputMax = br.ReadSingle();
        ValueMax = br.ReadArray(br.ReadSingle, 2);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Type);
        bw.Write(InputMin);
        bw.WriteArray(ValueMin, bw.Write);
        bw.Write(InputMax);
        bw.WriteArray(ValueMax, bw.Write);
    }
}