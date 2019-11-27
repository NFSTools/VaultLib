using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(FormationType))]
    public enum FormationType
    {
        PIT = 1,
        BOX_IN,
        ROLLING_BLOCK,
        FOLLOW,
        HELI_PURSUIT,
        HERD,
        ROLLING_BLOCK_LARGE,
        CHASE_UP_FRONT,
        CHASE_DIAGONAL,
        CHASE_RIGHT_TRIANGLE,
        CHASE_TRIANGLE,
        CHASE_REVERSE_TRIANGLE,
        REAR_RAM,
        SIDE_RAM,
        FRONT_RAM
    }
}
