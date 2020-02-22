using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.GRace
{
    [VLTTypeInfo("GRace::Type")]
    public enum Type
    {
        kEventType_None = -1,
        kEventType_P2P = 0x0,
        kEventType_Circuit = 0x1,
        kEventType_Checkpoint = 0x2,
        kEventType_CopsAndRobbers = 0x3,
        kEventType_OutRun = 0x4,
        kEventType_HighWayBattle = 0x5,
        kEventType_Challenge = 0x6,
        kEventType_Challenge_Bounty = 0x7,
        kEventType_Challenge_Cop_Takeout = 0x8,
        kEventType_Challenge_Escape = 0x9,
        kEventType_Mission = 0xA,
        kEventType_Shop = 0xB,
        kEventType_NotSupported = 0xC,
        kEventType_NumTypes = 0xD,
    }
}