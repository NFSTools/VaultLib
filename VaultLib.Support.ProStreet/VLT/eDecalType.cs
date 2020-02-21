using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(eDecalType))]
    public enum eDecalType
    {
        DECAL_TYPE_NONE = 0xFF,
        DECAL_TYPE_SKID_CONCRETE = 0,
        DECAL_TYPE_SKID_DIRT,
        DECAL_TYPE_SKID_GRASS,
        DECAL_TYPE_SKID_SAND,
        DECAL_TYPE_SCRAPE_METAL,
        DECAL_TYPE_SCRAPE_PLASTIC,
        DECAL_TYPE_SCRAPE_WOOD,
        DECAL_TYPE_SCRAPE_CONCRETE,
    }
}
