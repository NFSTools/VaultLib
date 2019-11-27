using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore
{
    [VLTTypeInfo("GameCore::MilestoneType")]
    public enum MilestoneType
    {
        kMilestone_None,
        kMilestone_General_AirTime,
        kMilestone_General_TopSpeed,
        kMilestone_General_FastestTopSpeed,
        kMilestone_General_NoPowerupsUsed,
        kMilestone_General_PowerupsUsed,
        kMilestone_General_PerfectStart,
        kMilestone_Circuit_BestLapTime,
        kMilestone_Pursuit_CopsDeployed,
        kMilestone_Pursuit_CopsEvaded,
        kMilestone_Pursuit_CopsRammed,
        kMilestone_Pursuit_CopsDestroyed,
        kMilestone_Pursuit_RhinosDestroyed,
        kMilestone_Pursuit_SpikestripsEvaded,
        kMilestone_Pursuit_RoadblocksEvaded,
        kMilestone_Pursuit_CostToState,
        kMilestone_Pursuit_Infractions,
        kMilestone_Pursuit_Bounty,
        kMilestone_Pursuit_Heat,
        kMilestone_Pursuit_Time,
        kMilestone_Pursuit_RhinoDisabled,
        kMilestone_Infraction_Speeding,
        kMilestone_Infraction_Racing,
        kMilestone_Infraction_RecklessDriving,
        kMilestone_Infraction_AssaultPolice,
        kMilestone_Infraction_HitAndRun,
        kMilestone_Infraction_DamageToProperty,
        kMilestone_Infraction_ResistingArrest,
        kMilestone_Infraction_DrivingOffRoad,
        kMilestone_Safehouse_Reached,
        kMilestone_Count,
    }
}
