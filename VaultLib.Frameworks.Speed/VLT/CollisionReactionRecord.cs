// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:27 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CollisionReactionRecord))]
    public struct CollisionReactionRecord
    {
        public float Elasticity;
        public float RollHeight;
        public float WeightBias;
        public float MassScale;
        public float StunSpeed;
        public float StunTime;
    }
}