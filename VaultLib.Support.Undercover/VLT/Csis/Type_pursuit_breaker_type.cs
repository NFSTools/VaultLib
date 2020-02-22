using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_pursuit_breaker_type")]
    public enum Type_pursuit_breaker_type
    {
        Invalid_Type_pursuit_breaker_type = 0x0,
        Type_pursuit_breaker_type_generic = 0x1,
        Type_pursuit_breaker_type_lumbermill = 0x2,
        Type_pursuit_breaker_type_reservior = 0x4,
        Type_pursuit_breaker_type_metal_refinery = 0x8,
        Type_pursuit_breaker_type_quarry = 0x10,
        Type_pursuit_breaker_type_dam = 0x20,
        Type_pursuit_breaker_type_freight_yard = 0x40,
        Type_pursuit_breaker_type_monorail = 0x80,
        Type_pursuit_breaker_type_spillway = 0x100,
        Type_pursuit_breaker_type_construction_site_causeway = 0x200,
        Type_pursuit_breaker_type_rock_wall = 0x400,
        Type_pursuit_breaker_type_gas_station = 0x800,
        Type_pursuit_breaker_type_electrical_tower = 0x1000,
        Type_pursuit_breaker_type_crane = 0x2000,
        Type_pursuit_breaker_type_interstate_sign = 0x4000,
        Type_pursuit_breaker_type_building = 0x8000,
        Type_pursuit_breaker_type_heavy_machinery = 0x10000,
        Type_pursuit_breaker_type_scaffolding = 0x20000,
        Type_pursuit_breaker_type_semi_truck = 0x40000,
    }
}