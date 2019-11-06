using VaultLib.Core.Types;

namespace VaultLib.Support.Carbon.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_SoundFX_Type")]
    public enum Type_SoundFX_Type
    {
        Invalid_Type_SoundFX_Type = 0x0,
        Type_SoundFX_Type_Pursbreaker = 0x1,
        Type_SoundFX_Type_Crash = 0x2,
        Type_SoundFX_Type_Thunder = 0x4,
        Type_SoundFX_Type_engine = 0x8,
        Type_SoundFX_Type_Car = 0x10,
        Type_SoundFX_Type_GamePlayMoment = 0x20,
        Type_SoundFX_Type_jump = 0x40,
        Type_SoundFX_Type_game = 0x80,
        Type_SoundFX_Type_Speedbreaker = 0x100,
    }
}
