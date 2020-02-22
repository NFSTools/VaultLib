using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Game
{
    [VLTTypeInfo("Game::GameMode")]
    public enum GameMode
    {
        kMode_Career = 0x0,
        kMode_SkipFE = 0x1,
        kMode_Online = 0x2,
        kMode_QuickRace = 0x3,
        kMode_SplitScreen = 0x4,
        kMode_CareerIntro = 0x5,
        kMode_Last = 0x6,
    }
}