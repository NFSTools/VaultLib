using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(FECarPartInfo))]
public struct FECarPartInfo
{
    public eFEPartUpgradeLevels Level;
    public float Cost;
}