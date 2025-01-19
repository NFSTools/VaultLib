// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 12:35 AM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(DamageScaleRecord))]
    public struct DamageScaleRecord
    {
        public float VisualScale;
        public float HitPointScale;
    }
}