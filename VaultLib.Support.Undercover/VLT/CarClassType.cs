using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(CarClassType))]
    public enum CarClassType
    {
        CarClassType_Racing = 0x0,
        CarClassType_Cop = 0x1,
        CarClassType_Traffic = 0x2,
    }
}