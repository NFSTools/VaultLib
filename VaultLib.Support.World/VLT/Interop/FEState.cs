using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Interop
{
    [VLTTypeInfo("Interop::FEState")]
    public enum FEState
    {
        FESTATE_INITIAL,
        FESTATE_FREEROAM,
        FESTATE_SAFEHOUSE,
        FESTATE_EVENT,
        FESTATE_PURSUIT,
        FESTATE_DISCONNECTED,
        FESTATE_LOBBY,
        FESTATE_FINAL,
        FESTATE_COUNT,
    }
}
