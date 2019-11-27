using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(ROLLFX_LOOP))]
    public enum ROLLFX_LOOP
    {
        ROLLFX_NONE = 0x0,
        ROLLFX_ASPHALT = 0x1,
        ROLLFX_CONCRETE = 0x2,
        ROLLFX_GRASS = 0x3,
        ROLLFX_SAND = 0x4,
        ROLLFX_GRAVEL = 0x5,
        ROLLFX_DIRT = 0x6,
        ROLLFX_WOOD = 0x7,
        ROLLFX_BRIDGE = 0x8,
        ROLLFX_BUMPY_HIGHWAY = 0x9,
        MAX_NUM_ROLLFX = 0xA,
    }
}
