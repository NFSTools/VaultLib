using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::RewardMode")]
    public enum RewardMode
    {
        kRewardMode_Singleplayer = 1,
        kRewardMode_Multiplayer = 2,
        kRewardMode_PrivateMatch = 3,
    }
}
