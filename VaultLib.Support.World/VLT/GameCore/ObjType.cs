using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::ObjType")]
    public enum ObjType
    {
        kObjType_Unknown = -1,
        kObjType_TrackLayout = 0,
        kObjType_Event = 1,
        kObjType_Trigger = 2,
        kObjType_Marker = 3,
        kObjType_Character = 4,
        kObjType_Milestone = 5,
        kObjType_PointOfInterestDefinition = 6,
        kObjType_Count = 7,
    }
}
