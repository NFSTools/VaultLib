using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.Physics.Info
{
    [VLTTypeInfo("Physics::Info::eInductionType")]
    public enum eInductionType
    {
        INDUCTION_NONE = 0x0,
        INDUCTION_TURBO_CHARGER = 0x1,
        INDUCTION_ROOTS_BLOWER = 0x2,
        INDUCTION_CENTRIFUGAL_BLOWER = 0x3,
        INDUCTION_TWINSCREW_BLOWER = 0x4,
    }
}