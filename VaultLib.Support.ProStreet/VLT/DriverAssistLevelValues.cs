using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(DriverAssistLevelValues))]
public class DriverAssistLevelValues : VltBaseType<Core.DataInterfaces.Key32>
{
    public int TractionControlLevel { get; set; }
    public int AntilockBrakeLevel { get; set; }
    public int StabilityControlLevel { get; set; }
    public int RaceLineAssist { get; set; }
    public int BrakingAssist { get; set; }
    public int DriftAssist { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        TractionControlLevel = br.ReadInt32();
        AntilockBrakeLevel = br.ReadInt32();
        StabilityControlLevel = br.ReadInt32();
        RaceLineAssist = br.ReadInt32();
        BrakingAssist = br.ReadInt32();
        DriftAssist = br.ReadInt32();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(TractionControlLevel);
        bw.Write(AntilockBrakeLevel);
        bw.Write(StabilityControlLevel);
        bw.Write(RaceLineAssist);
        bw.Write(BrakingAssist);
        bw.Write(DriftAssist);
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}