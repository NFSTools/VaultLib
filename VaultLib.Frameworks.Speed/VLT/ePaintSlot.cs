using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(ePaintSlot))]
    public enum ePaintSlot
    {
        CUSTOM_PAINT_SLOT_BODY = 0x0,
        CUSTOM_PAINT_SLOT_RIMS_FRONT = 0x1,
        CUSTOM_PAINT_SLOT_RIMS_REAR = 0x2,
        CUSTOM_PAINT_SLOT_ROLLCAGE = 0x3,
        CUSTOM_PAINT_SLOT_BUMPER_FRONT = 0x4,
        CUSTOM_PAINT_SLOT_BUMPER_REAR = 0x5,
        CUSTOM_PAINT_SLOT_SIDESKIRT = 0x6,
        CUSTOM_PAINT_SLOT_HOOD = 0x7,
        CUSTOM_PAINT_SLOT_MIRROR = 0x8,
        CUSTOM_PAINT_SLOT_SPOILER = 0x9,
        CUSTOM_PAINT_SLOT_WINDOW_TINT = 0xA,
        CUSTOM_PAINT_SLOT_ROOFSCOOP = 0xB,
        MAX_CUSTOM_PAINT_SLOTS = 0xC,
        INVALID_PAINT_SLOT = 0xD,
    }
}