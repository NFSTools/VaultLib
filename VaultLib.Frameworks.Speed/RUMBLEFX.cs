using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(RUMBLEFX))]
    public enum RUMBLEFX
    {
        RUMBLEFX_NONE = 0x0,
        RUMBLEFX_ROUGH = 0x1,
        RUMBLEFX_SMOOTH = 0x2,
        RUMBLEFX_STEP = 0x3,
        MAX_RUMBLEFX = 0x4,
    }
}
