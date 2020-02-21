using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT.Csis
{
    [VLTTypeInfo("Csis::Type_CarTypeC")]
    public enum Type_CarTypeC
    {
        Invalid_Type_CarTypeC = 0x0,
        Type_CarTypeC_Koeniggsigg_CCX = 0x1,
        Type_CarTypeC_gallardo = 0x2,
        Type_CarTypeC_lancia_integrale_1993 = 0x4,
        Type_CarTypeC_mclaren_f1 = 0x8,
        Type_CarTypeC_mercedes_sl65 = 0x10,
        Type_CarTypeC_mugen_civic = 0x20,
        Type_CarTypeC_roadrunner_1969 = 0x40,
        Type_CarTypeC_porsche_997_gt3 = 0x80,
        Type_CarTypeC_porschegt3_rs = 0x100,
        Type_CarTypeC_porsche_carrera_GT = 0x200,
        Type_CarTypeC_SEAT_leon_cupra = 0x400,
    }
}