using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.GRace
{
    [VLTTypeInfo("GRace::AIOpponentGoal")]
    public enum AIOpponentGoal
    {
        kAIGoalBully = 0x0,
        kAIGoalLead = 0x1,
        kAIGoalEvade = 0x2,
        kAIGoalChase = 0x3,
        kAIGoalRacer = 0x4,
        kAIGoalWaypointRacer = 0x5,
    }
}