using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(HighwayPatternCarData))]
public class HighwayPatternCarData : VltBaseType<Core.DataInterfaces.Key32>
{
    public int Row { get; set; }
    public int Lane { get; set; }
    public RefSpec32 Vehicle { get; set; } = new();
    public EAILaneChangeType Change { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Row = br.ReadInt32();
        Lane = br.ReadInt32();
        Vehicle.Read(context, fieldContext, br);
        Change = br.ReadEnum<EAILaneChangeType>();

        var v = br.ReadUInt32();
        if (v != 0)
            throw new InvalidDataException();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(Row);
        bw.Write(Lane);
        Vehicle.Write(context, fieldContext, bw);
        bw.WriteEnum(Change);
        bw.Write(0);
    }

    public override object Clone()
    {
        return new HighwayPatternCarData
        {
            Row = Row,
            Lane = Lane,
            Vehicle = (RefSpec32)Vehicle.Clone(),
            Change = Change,
        };
    }
}