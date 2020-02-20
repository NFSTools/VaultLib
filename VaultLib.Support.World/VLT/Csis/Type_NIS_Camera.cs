using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_NIS_Camera")]
    public enum Type_NIS_Camera
    {
        Invalid_Type_NIS_Camera = 0x0,
        Type_NIS_Camera_CAMCANYON = 0x1,
        Type_NIS_Camera_CAMCLIFF = 0x2,
        Type_NIS_Camera_CAMLEG = 0x4,
        Type_NIS_Camera_CAMWIN = 0x8,
        Type_NIS_Camera_CAMLOSE = 0x10,
        Type_NIS_Camera_CAMLOSECANYON = 0x20,
        Type_NIS_Camera_CAMSPOTCREW = 0x40,
    }
}
