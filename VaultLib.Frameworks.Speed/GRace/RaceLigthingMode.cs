using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.GRace
{
    [VLTTypeInfo("GRace::RaceLigthingMode")]
    public enum RaceLigthingMode
    {
        kLighting_Normal = 0x0,
        kLightingDrift = 0x1,
        kLightingCanyon = 0x2,
    }
}