using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(EmitterPostProcessingEffect))]
    public enum EmitterPostProcessingEffect
    {
        EmitterPostProcessingEffect_NONE = 0x0,
        EmitterPostProcessingEffect_SMOKE = 0x1,
    }
}