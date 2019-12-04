// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:27 AM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(CollisionReactionRecord))]
    public class CollisionReactionRecord : VLTBaseType
    {
        public float Elasticity { get; set; }
        public float RollHeight { get; set; }
        public float WeightBias { get; set; }
        public float MassScale { get; set; }
        public float StunSpeed { get; set; }
        public float StunTime { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Elasticity = br.ReadSingle();
            RollHeight = br.ReadSingle();
            WeightBias = br.ReadSingle();
            MassScale = br.ReadSingle();
            StunSpeed = br.ReadSingle();
            StunTime = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Elasticity);
            bw.Write(RollHeight);
            bw.Write(WeightBias);
            bw.Write(MassScale);
            bw.Write(StunSpeed);
            bw.Write(StunTime);
        }

        public CollisionReactionRecord(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public CollisionReactionRecord(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}