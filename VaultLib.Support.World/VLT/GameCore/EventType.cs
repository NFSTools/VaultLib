using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::EventType")]
    public enum EventType
    {
        kEventType_Unknown,
        kEventType_P2P,
        kEventType_Circuit,
        kEventType_Pursuit,
        kEventType_MeetingPlace,
        kEventType_TeamEscape,
        kEventType_Drag,
    }
}
