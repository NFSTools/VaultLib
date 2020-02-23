using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(PartType))]
    public enum PartType
    {
        PartType_CarSpecific = 0x0,
        PartType_Universal = 0x1,
        PartType_Vinyl = 0x2,
        PartType_Paint = 0x3,
        PartType_Windtunnel = 0x4,
        PartType_Damage = 0x5,
        PartType_DamageSD = 0x6,
    }
}