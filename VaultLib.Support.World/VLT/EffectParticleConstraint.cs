// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:40 AM.

using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(EffectParticleConstraint))]
    public enum EffectParticleConstraint
    {
        CONSTRAIN_PARTICLE_NONE = 0x0,
        CONSTRAIN_PARTICLE_XY_AXIS = 0x1,
        CONSTRAIN_PARTICLE_XZ_AXIS = 0x2,
        CONSTRAIN_PARTICLE_YZ_AXIS = 0x3,
        CONSTRAIN_PARTICLE_SPRAY_SIDEWAYS = 0x4,
        CONSTRAIN_PARTICLE_SPRAY_HEADON = 0x5,
    }
}