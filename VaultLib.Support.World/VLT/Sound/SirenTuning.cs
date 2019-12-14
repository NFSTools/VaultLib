using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound
{
    [VLTTypeInfo("Sound::SirenTuning")]
    public class SirenTuning : VLTBaseType
    {
        public float[] OpRadiusLimit { get; set; }
        public float SpeedThresh { get; set; }
        public float[] HornLimit { get; set; }
        public float[] PriorityLimit { get; set; }
        public float[] WailLimit { get; set; }
        public float[] YelpLimit { get; set; }
        public float[] LoopXFadeRange { get; set; }
        
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public float Unknown4 { get; set; }
        
        public override void Read(Vault vault, BinaryReader br)
        {
            // TODO: investigate structure
            
            OpRadiusLimit = new float[2];
            HornLimit = new float[2];
            PriorityLimit = new float[2];
            WailLimit = new float[2];
            YelpLimit = new float[2];
            LoopXFadeRange = new float[2];

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

        public override void Write(Vault vault, BinaryWriter bw)
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

        public SirenTuning(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public SirenTuning(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}