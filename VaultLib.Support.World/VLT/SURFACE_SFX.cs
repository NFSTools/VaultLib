using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(SURFACE_SFX))]
    public enum SURFACE_SFX
    {
        SURFACE_SFX_NONE = 0x0,
        SURFACE_SFX_LIGHT_CRACK = 0x1,
        SURFACE_SFX_TAR_STRIP = 0x3,
        SURFACE_SFX_ROADSIDE_PATCH = 0x5,
        SURFACE_SFX_HEAVY_PATCH = 0x6,
        SURFACE_SFX_DIRT = 0x7,
        MAX_SURFACE_SFX = 0x8,
    }
}
