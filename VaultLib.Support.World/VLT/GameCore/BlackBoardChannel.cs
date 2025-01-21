using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::BlackBoardChannel")]
public enum BlackBoardChannel
{
    kBlackBoard_Audio,
    kBlackBoard_Frontend,
    kBlackBoard_AI,
    kBlackBoard_WorldMap,
    kBlackBoard_Count,
}