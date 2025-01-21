using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(RoadblockSetup))]
public class RoadblockSetup : VltBaseType
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

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
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

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
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
}