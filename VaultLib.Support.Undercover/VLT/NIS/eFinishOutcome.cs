using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::eFinishOutcome")]
    public enum eFinishOutcome
    {
        FINISH_WIN = 0x0,
        FINISH_LOSE = 0x1,
        FINISH_INVALID = 0x2,
    }
}