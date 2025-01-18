// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:27 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CollisionReactionRecord))]
    public class CollisionReactionRecord : VltBaseType
    {
        public float Elasticity { get; set; }
        public float RollHeight { get; set; }
        public float WeightBias { get; set; }
        public float MassScale { get; set; }
        public float StunSpeed { get; set; }
        public float StunTime { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Elasticity = br.ReadSingle();
            RollHeight = br.ReadSingle();
            WeightBias = br.ReadSingle();
            MassScale = br.ReadSingle();
            StunSpeed = br.ReadSingle();
            StunTime = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Elasticity);
            bw.Write(RollHeight);
            bw.Write(WeightBias);
            bw.Write(MassScale);
            bw.Write(StunSpeed);
            bw.Write(StunTime);
        }
    }
}