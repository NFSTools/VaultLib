using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Sound
{
    [VLTTypeInfo("Sound::ChasedownMusicTuning")]
    public class ChasedownMusicTuning : VLTBaseType
    {
        public float[] StartTimeLimit { get; set; }
        public float[] LowTimeLimit { get; set; }
        public float[] FailureTimeLimit { get; set; }
        public float[] MediumTimeLimit { get; set; }
        public float[] HighTimeLimit { get; set; }
        public float[] OpponentDamageThreshold { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            StartTimeLimit = new float[2];
            LowTimeLimit = new float[2];
            FailureTimeLimit = new float[2];
            MediumTimeLimit = new float[2];
            HighTimeLimit = new float[2];
            OpponentDamageThreshold = new float[2];

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

        public override void Write(Vault vault, BinaryWriter bw)
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

        public ChasedownMusicTuning(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public ChasedownMusicTuning(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}