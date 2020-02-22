namespace VaultLib.Support.Undercover.VLT.RenderReflect
{
    public enum State_GPUFunc
    {
        kStateGPUFunc_Never = 0x0,
        kStateGPUFunc_Less = 0x1,
        kStateGPUFunc_Equal = 0x2,
        kStateGPUFunc_Less_Equal = 0x3,
        kStateGPUFunc_Greater = 0x4,
        kStateGPUFunc_Not_Equal = 0x5,
        kStateGPUFunc_Greater_Equal = 0x6,
        kStateGPUFunc_Always = 0x7,
    }
}