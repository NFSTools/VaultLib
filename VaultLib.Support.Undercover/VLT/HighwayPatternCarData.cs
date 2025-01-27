using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(HighwayPatternCarData))]
public class HighwayPatternCarData: VltBaseType<uint>
{
    public int Row { get; set; }
    public int Lane { get; set; }
    public RefSpec<uint> Vehicle { get; set; } = new();
    public EAILaneChangeType Change { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Row = br.ReadInt32();
        Lane = br.ReadInt32();
        Vehicle.Read(context, fieldContext, br);
        Change = br.ReadEnum<EAILaneChangeType>();

        var v = br.ReadUInt32();
        if (v != 0)
            throw new InvalidDataException();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(Row);
        bw.Write(Lane);
        Vehicle.Write(context, fieldContext, bw);
        bw.WriteEnum(Change);
        bw.Write(0);
    }
}