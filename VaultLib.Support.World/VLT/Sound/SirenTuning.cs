using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound;

[VltTypeInfo("Sound::SirenTuning")]
public class SirenTuning : VltBaseType<Core.DataInterfaces.Key32>
{
    public float[] OpRadiusLimit { get; set; } = new float[2];
    public float SpeedThresh { get; set; }
    public float[] HornLimit { get; set; } = new float[2];
    public float[] PriorityLimit { get; set; } = new float[2];
    public float[] WailLimit { get; set; } = new float[2];
    public float[] YelpLimit { get; set; } = new float[2];
    public float[] LoopXFadeRange { get; set; } = new float[2];

    public int Unknown1 { get; set; }
    public int Unknown2 { get; set; }
    public int Unknown3 { get; set; }
    public float Unknown4 { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        // TODO: investigate structure

        for (int i = 0; i < OpRadiusLimit.Length; i++)
        {
            OpRadiusLimit[i] = br.ReadSingle();
        }

        SpeedThresh = br.ReadSingle();

        for (int i = 0; i < HornLimit.Length; i++)
        {
            HornLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < PriorityLimit.Length; i++)
        {
            PriorityLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < WailLimit.Length; i++)
        {
            WailLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < YelpLimit.Length; i++)
        {
            YelpLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < LoopXFadeRange.Length; i++)
        {
            LoopXFadeRange[i] = br.ReadSingle();
        }

        Unknown1 = br.ReadInt32();
        Unknown2 = br.ReadInt32();
        Unknown3 = br.ReadInt32();
        Unknown4 = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        for (int i = 0; i < OpRadiusLimit.Length; i++)
        {
            bw.Write(OpRadiusLimit[i]);
        }

        bw.Write(SpeedThresh);

        for (int i = 0; i < HornLimit.Length; i++)
        {
            bw.Write(HornLimit[i]);
        }

        for (int i = 0; i < PriorityLimit.Length; i++)
        {
            bw.Write(PriorityLimit[i]);
        }

        for (int i = 0; i < WailLimit.Length; i++)
        {
            bw.Write(WailLimit[i]);
        }

        for (int i = 0; i < YelpLimit.Length; i++)
        {
            bw.Write(YelpLimit[i]);
        }

        for (int i = 0; i < LoopXFadeRange.Length; i++)
        {
            bw.Write(LoopXFadeRange[i]);
        }

        bw.Write(Unknown1);
        bw.Write(Unknown2);
        bw.Write(Unknown3);
        bw.Write(Unknown4);
    }

    public override object Clone()
    {
        return new SirenTuning
        {
            OpRadiusLimit = (float[])OpRadiusLimit.Clone(),
            SpeedThresh = SpeedThresh,
            HornLimit = (float[])HornLimit.Clone(),
            PriorityLimit = (float[])PriorityLimit.Clone(),
            WailLimit = (float[])WailLimit.Clone(),
            YelpLimit = (float[])YelpLimit.Clone(),
            LoopXFadeRange = (float[])LoopXFadeRange.Clone(),
            Unknown1 = Unknown1,
            Unknown2 = Unknown2,
            Unknown3 = Unknown3,
            Unknown4 = Unknown4,
        };
    }
}