// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:04 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound
{
    [VLTTypeInfo("Sound::PursuitMusicTuning")]
    public class PursuitMusicTuning : VLTBaseType
    {
        public float PlayerSpeedUR { get; set; }
        public float PursuitUR { get; set; }
        public float PursuitDistThresh { get; set; }
        public float Evade2OutrunSpeedThresh { get; set; }
        public float[] HighTimeLimit { get; set; }
        public float[] LowTimeLimit { get; set; }
        public float[] EvadeTimeLimit { get; set; }
        public float[] NormalTimeLimit { get; set; }
        public float[] OutrunTimeLimit { get; set; }
        public float[] SafeTimeLimit { get; set; }
        public float[] UnsafeTimeLimit { get; set; }
        public float StartupDelay { get; set; }
        public float PctMaxVel_Low { get; set; }
        public float MinTimeLost { get; set; }
        public float MinTimeLastCrashed { get; set; }
        public float PctMaxVel_High { get; set; }
        public float MaxPursuitDist { get; set; }
        public float MinCopCohesion { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            PlayerSpeedUR = br.ReadSingle();
            PursuitUR = br.ReadSingle();
            PursuitDistThresh = br.ReadSingle();
            Evade2OutrunSpeedThresh = br.ReadSingle();
            for (int i = 0; i < HighTimeLimit.Length; i++)
            {
                HighTimeLimit[i] = br.ReadSingle();
            }

            for (int i = 0; i < LowTimeLimit.Length; i++)
            {
                LowTimeLimit[i] = br.ReadSingle();
            }

            for (int i = 0; i < EvadeTimeLimit.Length; i++)
            {
                EvadeTimeLimit[i] = br.ReadSingle();
            }

            for (int i = 0; i < NormalTimeLimit.Length; i++)
            {
                NormalTimeLimit[i] = br.ReadSingle();
            }

            for (int i = 0; i < OutrunTimeLimit.Length; i++)
            {
                OutrunTimeLimit[i] = br.ReadSingle();
            }

            for (int i = 0; i < SafeTimeLimit.Length; i++)
            {
                SafeTimeLimit[i] = br.ReadSingle();
            }

            for (int i = 0; i < UnsafeTimeLimit.Length; i++)
            {
                UnsafeTimeLimit[i] = br.ReadSingle();
            }

            StartupDelay = br.ReadSingle();
            PctMaxVel_Low = br.ReadSingle();
            MinTimeLost = br.ReadSingle();
            MinTimeLastCrashed = br.ReadSingle();
            PctMaxVel_High = br.ReadSingle();
            MaxPursuitDist = br.ReadSingle();
            MinCopCohesion = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(PlayerSpeedUR);
            bw.Write(PursuitUR);
            bw.Write(PursuitDistThresh);
            bw.Write(Evade2OutrunSpeedThresh);

            for (int i = 0; i < HighTimeLimit.Length; i++)
            {
                bw.Write(HighTimeLimit[i]);
            }

            for (int i = 0; i < LowTimeLimit.Length; i++)
            {
                bw.Write(LowTimeLimit[i]);
            }

            for (int i = 0; i < EvadeTimeLimit.Length; i++)
            {
                bw.Write(EvadeTimeLimit[i]);
            }

            for (int i = 0; i < NormalTimeLimit.Length; i++)
            {
                bw.Write(NormalTimeLimit[i]);
            }

            for (int i = 0; i < OutrunTimeLimit.Length; i++)
            {
                bw.Write(OutrunTimeLimit[i]);
            }

            for (int i = 0; i < SafeTimeLimit.Length; i++)
            {
                bw.Write(SafeTimeLimit[i]);
            }

            for (int i = 0; i < UnsafeTimeLimit.Length; i++)
            {
                bw.Write(UnsafeTimeLimit[i]);
            }

            bw.Write(StartupDelay);
            bw.Write(PctMaxVel_Low);
            bw.Write(MinTimeLost);
            bw.Write(MinTimeLastCrashed);
            bw.Write(PctMaxVel_High);
            bw.Write(MaxPursuitDist);
            bw.Write(MinCopCohesion);
        }

        public PursuitMusicTuning(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            HighTimeLimit = new float[2];
            LowTimeLimit = new float[2];
            EvadeTimeLimit = new float[2];
            NormalTimeLimit = new float[2];
            OutrunTimeLimit = new float[2];
            SafeTimeLimit = new float[2];
            UnsafeTimeLimit = new float[2];
        }
    }
}