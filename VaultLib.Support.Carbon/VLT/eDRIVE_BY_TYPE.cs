using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(eDRIVE_BY_TYPE))]
    public enum eDRIVE_BY_TYPE
    {
        DRIVE_BY_UNKNOWN = 0x0,
        DRIVE_BY_TREE = 0x1,
        DRIVE_BY_LAMPPOST = 0x2,
        DRIVE_BY_SMOKABLE = 0x3,
        DRIVE_BY_TUNNEL_IN = 0x4,
        DRIVE_BY_TUNNEL_OUT = 0x5,
        DRIVE_BY_OVERPASS_IN = 0x6,
        DRIVE_BY_OVERPASS_OUT = 0x7,
        DRIVE_BY_AI_CAR = 0x8,
        DRIVE_BY_TRAFFIC = 0x9,
        DRIVE_BY_BRIDGE = 0xA,
        DRIVE_BY_PRE_COL = 0xB,
        DRIVE_BY_CAMERA_BY = 0xC,
        DRIVE_BY_POLE_SKINNY = 0xD,
        DRIVE_BY_SUPPORT_OVERPASS = 0xE,
        DRIVE_BY_BILLBOARD = 0xF,
        DRIVE_BY_STRUCTURE = 0x10,
        DRIVE_BY_STAGING = 0x11,
        MAX_DRIVE_BY_TYPES = 0x12,
    }
}
