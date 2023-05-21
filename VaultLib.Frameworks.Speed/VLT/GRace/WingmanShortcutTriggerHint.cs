using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.GRace
{
    [VLTTypeInfo("GRace::WingmanShortcutTriggerHint")]
    public enum WingmanShortcutTriggerHint
    {
        kShortcutTrigger_None = 0x0,
        kShortcutTrigger_Init = 0x1,
        kShortcutTrigger_Entry = 0x2,
        kShortcutTrigger_Pos = 0x3,
        kShortcutTrigger_Exit = 0x4,
        kShortcutTrigger_All = 0x5,
    }
}