using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.Csis
{
    [VLTTypeInfo("Csis::Type_SoundFX_Param")]
    public enum Type_SoundFX_Param
    {
        Invalid_Type_SoundFX_Param = 0x0,
        Type_SoundFX_Param_distant = 0x1,
        Type_SoundFX_Param_sweetener = 0x2,
        Type_SoundFX_Param_blow = 0x4,
        Type_SoundFX_Param_total = 0x8,
        Type_SoundFX_Param_expl_gas_station = 0x10,
        Type_SoundFX_Param_truck_jackknife = 0x20,
        Type_SoundFX_Param_boatfall = 0x40,
        Type_SoundFX_Param_trck_log_dump = 0x80,
        Type_SoundFX_Param_stripmall = 0x100,
        Type_SoundFX_Param_scaffolding_big = 0x200,
        Type_SoundFX_Param_drive_in = 0x400,
        Type_SoundFX_Param_water_tower = 0x800,
        Type_SoundFX_Param_porch_collapse = 0x1000,
        Type_SoundFX_Param_comm_tower = 0x2000,
        Type_SoundFX_Param_fish_market_sign = 0x4000,
        Type_SoundFX_Param_patio_collapse = 0x8000,
        Type_SoundFX_Param_start_music = 0x10000,
        Type_SoundFX_Param_end_music = 0x20000,
        Type_SoundFX_Param_start_effects = 0x40000,
        Type_SoundFX_Param_end_effects = 0x80000,
        Type_SoundFX_Param_cam = 0x100000,
        Type_SoundFX_Param_break = 0x200000,
        Type_SoundFX_Param_amphitheatre = 0x400000,
        Type_SoundFX_Param_gate_signage = 0x800000,
        Type_SoundFX_Param_trailer_park = 0x1000000,
        Type_SoundFX_Param_torus_shop = 0x2000000,
        Type_SoundFX_Param_gazebo = 0x4000000,
        Type_SoundFX_Param_uves = 0x8000000,
        Type_SoundFX_Param_zoom = 0x10000000,
        Type_SoundFX_Param_camera = 0x20000000,
        Type_SoundFX_Param_police_tower = 0x40000000,
    }
}
