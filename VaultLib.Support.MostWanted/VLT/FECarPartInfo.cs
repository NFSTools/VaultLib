using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.MostWanted.VLT;

[VltTypeInfo(nameof(FECarPartInfo))]
public class FECarPartInfo: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public eFEPartUpgradeLevels Level { get; set; }
    public float Unknown { get; set; }
    public float Cost { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Level = br.ReadEnum<eFEPartUpgradeLevels>();
        Unknown = br.ReadSingle();
        Cost = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Level);
        bw.Write(Unknown);
        bw.Write(Cost);
    }
}