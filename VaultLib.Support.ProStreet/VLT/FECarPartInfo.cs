using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FECarPartInfo))]
public class FECarPartInfo: VltBaseType<Core.DataInterfaces.Key32>
{
    public eFEPartUpgradeLevels Level { get; set; }
    public float Cost { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Level = br.ReadEnum<eFEPartUpgradeLevels>();
        Cost = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context, FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Level);
        bw.Write(Cost);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}