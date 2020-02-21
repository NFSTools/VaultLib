using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(SPEECH_CAR_GENERATION))]
    public enum SPEECH_CAR_GENERATION
    {
        InvalidCarGeneration = 0x0,
        CarGeneration_Acura_DC5 = 0x1,
        CarGeneration_Integra = 0x2,
        CarGeneration_Corvette = 0x3,
        CarGeneration_Z06 = 0x4,
        CarGeneration_Evo_X = 0x5,
        CarGeneration_Evo_IX_MR = 0x6,
        CarGeneration_GT_500 = 0x7,
        CarGeneration_Shelby_GT_500 = 0x8,
        CarGeneration_Skyline_R34 = 0x9,
        CarGeneration_Skyline_R35 = 0xA,
        CarGeneration_GTO_2006 = 0xB,
        CarGeneration_GT0_1965 = 0xC,
    }
}