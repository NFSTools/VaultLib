using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.NIS
{
    [VLTTypeInfo("NIS::eOnlineStatus")]
    public enum eOnlineStatus
    {
        ONLINE_ONLY = 0x0,
        ONLINE_DISABLED = 0x1,
        ONLINE_DOES_NOT_MATTER = 0x2,
    }
}