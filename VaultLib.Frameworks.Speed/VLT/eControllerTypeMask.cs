using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(eControllerTypeMask))]
public enum eControllerTypeMask
{
    CTM_ANY = 0x0,
    CTM_WII_REMOTE = 0x1,
    CTM_WII_NUNCHUK = 0x3,
    CTM_WII_CLASSIC = 0x5,
}