using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FECarPartInfo))]
public class FECarPartInfo: VltBaseType<uint>
{
    public eFEPartUpgradeLevels Level { get; set; }
    public float Cost { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Level = br.ReadEnum<eFEPartUpgradeLevels>();
        Cost = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(Level);
        bw.Write(Cost);
    }
}