using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CameraReactionRecord))]
public class CameraReactionRecord : VltBaseType<Key32>
{
    public CameraReactionType Type { get; set; }
    public float InputMin { get; set; }
    public float[] ValueMin { get; set; } = new float[2];
    public float InputMax { get; set; }
    public float[] ValueMax { get; set; } = new float[2];

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Type = br.ReadEnum<CameraReactionType>();
        InputMin = br.ReadSingle();
        ValueMin = br.ReadArray(br.ReadSingle, 2);
        InputMax = br.ReadSingle();
        ValueMax = br.ReadArray(br.ReadSingle, 2);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        bw.WriteEnum(Type);
        bw.Write(InputMin);
        bw.WriteArray(ValueMin, bw.Write);
        bw.Write(InputMax);
        bw.WriteArray(ValueMax, bw.Write);
    }

    public override object Clone()
    {
        return new CameraReactionRecord
        {
            Type = Type,
            InputMin = InputMin,
            InputMax = InputMax,
            ValueMin = (float[])ValueMin.Clone(),
            ValueMax = (float[])ValueMax.Clone(),
        };
    }
}