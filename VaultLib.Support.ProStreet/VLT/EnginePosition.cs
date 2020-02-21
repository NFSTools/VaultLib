using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(EnginePosition))]
    public enum EnginePosition
    {
        ENGINE_REAR = 0x0,
        ENGINE_MID = 0x1,
        ENGINE_FRONT = 0x2,
    }
}