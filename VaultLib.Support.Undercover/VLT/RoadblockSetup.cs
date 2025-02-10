using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(RoadblockSetup))]
public class RoadblockSetup : VltBaseType<Core.DataInterfaces.Key32>
{
    public RoadblockSetup()
    {
        Contents = new RoadblockElement[6];
        for (var i = 0; i < 6; i++)
            Contents[i] = new RoadblockElement();
    }

    public float MinimumWidthRequired { get; set; }
    public uint RequiredVehicles { get; set; }
    public float MinimumThreatLevel { get; set; }
    public float MaximumThreatLevel { get; set; }
    public RoadblockElement[] Contents { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        MinimumWidthRequired = br.ReadSingle();
        RequiredVehicles = br.ReadUInt32();
        MinimumThreatLevel = br.ReadSingle();
        MaximumThreatLevel = br.ReadSingle();

        for (int i = 0; i < 6; i++)
        {
            Contents[i].Read(context, fieldContext, br);
        }
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(MinimumWidthRequired);
        bw.Write(RequiredVehicles);
        bw.Write(MinimumThreatLevel);
        bw.Write(MaximumThreatLevel);

        for (int i = 0; i < 6; i++)
        {
            Contents[i].Write(context, fieldContext, bw);
        }
    }

    public override object Clone()
    {
        return new RoadblockSetup
        {
            MinimumWidthRequired = MinimumWidthRequired,
            RequiredVehicles = RequiredVehicles,
            MinimumThreatLevel = MinimumThreatLevel,
            MaximumThreatLevel = MaximumThreatLevel,
            Contents = Contents.CloneComplex()
        };
    }
}