using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(ICEContextIDs))]
    public enum ICEContextIDs
    {
        eICE_NISProto = 0x0,
        eICE_NISFinish = 0x1,
        eICE_NISRaceIntros = 0x2,
        eICE_NISUserArrested = 0x3,
        eICE_NISPursuitBreaker = 0x4,
        eICE_NISDriverJobs = 0x5,
        eICE_FMV = 0x6,
        eICE_MARKETING = 0x7,
        eICE_REPLAY = 0x8,
        eICE_RECORDED = 0x9,
        eICE_GENERIC = 0xA,
        eICE_FE = 0xB,
    }
}