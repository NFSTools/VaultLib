using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(KIT_TYPE))]
    public enum KIT_TYPE
    {
        KIT_TYPE_BASE = 0x0,
        KIT_TYPE_STOCK = 0x1,
        KIT_TYPE_AUTOSCULPT = 0x2,
        KIT_TYPE_WIDEBODY = 0x3,
        NUM_KIT_TYPE = 0x4,
    }
}