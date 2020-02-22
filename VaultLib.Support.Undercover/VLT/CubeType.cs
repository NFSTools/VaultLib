using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(CubeType))]
    public enum CubeType
    {
        CubeType_FILTER0 = 0x0,
        CubeType_TRANSFORMERS = 0x1,
        CubeType_GONE_IN_60_SECONDS = 0x2,
        CubeType_INTHEZONE = 0x3,
        CubeType_COPINTRO = 0x4,
        CubeType_PAUSETREATMENT = 0x5,
        CubeType_LEVELUP = 0x6,
        CubeType_WHITEOVERLAY = 0x7,
        CubeType_DESTINATION1 = 0x8,
        CubeType_COPAPPROACHING = 0x9,
        CubeType_NUM = 0xA,
    }
}