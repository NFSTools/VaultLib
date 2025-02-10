using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT.Sound;

[VltTypeInfo("Sound::ChasedownMusicTuning")]
public class ChasedownMusicTuning : VltBaseType<Core.DataInterfaces.Key32>
{
    public float[] StartTimeLimit { get; set; } = new float[2];
    public float[] LowTimeLimit { get; set; } = new float[2];
    public float[] FailureTimeLimit { get; set; } = new float[2];
    public float[] MediumTimeLimit { get; set; } = new float[2];
    public float[] HighTimeLimit { get; set; } = new float[2];
    public float[] OpponentDamageThreshold { get; set; } = new float[2];

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        for (int i = 0; i < StartTimeLimit.Length; i++)
        {
            StartTimeLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < LowTimeLimit.Length; i++)
        {
            LowTimeLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < FailureTimeLimit.Length; i++)
        {
            FailureTimeLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < MediumTimeLimit.Length; i++)
        {
            MediumTimeLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < HighTimeLimit.Length; i++)
        {
            HighTimeLimit[i] = br.ReadSingle();
        }

        for (int i = 0; i < OpponentDamageThreshold.Length; i++)
        {
            OpponentDamageThreshold[i] = br.ReadSingle();
        }
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        for (int i = 0; i < StartTimeLimit.Length; i++)
        {
            bw.Write(StartTimeLimit[i]);
        }

        for (int i = 0; i < LowTimeLimit.Length; i++)
        {
            bw.Write(LowTimeLimit[i]);
        }

        for (int i = 0; i < FailureTimeLimit.Length; i++)
        {
            bw.Write(FailureTimeLimit[i]);
        }

        for (int i = 0; i < MediumTimeLimit.Length; i++)
        {
            bw.Write(MediumTimeLimit[i]);
        }

        for (int i = 0; i < HighTimeLimit.Length; i++)
        {
            bw.Write(HighTimeLimit[i]);
        }

        for (int i = 0; i < OpponentDamageThreshold.Length; i++)
        {
            bw.Write(OpponentDamageThreshold[i]);
        }
    }

    public override object Clone()
    {
        return new ChasedownMusicTuning
        {
            StartTimeLimit = StartTimeLimit.CloneSimple(),
            LowTimeLimit = LowTimeLimit.CloneSimple(),
            FailureTimeLimit = FailureTimeLimit.CloneSimple(),
            MediumTimeLimit = MediumTimeLimit.CloneSimple(),
            HighTimeLimit = HighTimeLimit.CloneSimple(),
            OpponentDamageThreshold = OpponentDamageThreshold.CloneSimple(),
        };
    }
}