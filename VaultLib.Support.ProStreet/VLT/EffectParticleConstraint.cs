using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(EffectParticleConstraint))]
    public enum EffectParticleConstraint
    {
        CONSTRAIN_PARTICLE_NONE = 0x0,
        CONSTRAIN_PARTICLE_XY_AXIS = 0x6,
        CONSTRAIN_PARTICLE_XZ_AXIS = 0x5,
        CONSTRAIN_PARTICLE_YZ_AXIS = 0x3,
        CONSTRAIN_PARTICLE_CAMERA = 0x8,
    }
}