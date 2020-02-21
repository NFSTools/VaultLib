using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_SameMake")]
    public enum Type_SameMake
    {
        Invalid_Type_SameMake = 0x0,
        Type_SameMake_generic = 0x1,
        Type_SameMake_Acura = 0x2,
        Type_SameMake_Audi = 0x4,
        Type_SameMake_BMW = 0x8,
        Type_SameMake_Chevrolet = 0x10,
        Type_SameMake_Dodge = 0x20,
        Type_SameMake_Ford = 0x40,
        Type_SameMake_Honda = 0x80,
        Type_SameMake_Lamborghini = 0x100,
        Type_SameMake_Mazda = 0x200,
        Type_SameMake_Mitsubishi = 0x400,
        Type_SameMake_Nissan = 0x800,
        Type_SameMake_Pontiac = 0x1000,
        Type_SameMake_Porsche = 0x2000,
        Type_SameMake_Toyota = 0x4000,
        Type_SameMake_Volkswagon = 0x8000,
    }
}