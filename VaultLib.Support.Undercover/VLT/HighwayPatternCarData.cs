using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(HighwayPatternCarData))]
    public class HighwayPatternCarData : VltBaseType
    {
        public HighwayPatternCarData(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Vehicle = new RefSpec(Class, Field, Collection);
        }

        public int Row { get; set; }
        public int Lane { get; set; }
        public RefSpec Vehicle { get; set; }
        public EAILaneChangeType Change { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Row = br.ReadInt32();
            Lane = br.ReadInt32();
            Vehicle.Read(context, fieldContext, br);
            Change = br.ReadEnum<EAILaneChangeType>();

            var v = br.ReadUInt32();
            if (v != 0)
                throw new InvalidDataException();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Row);
            bw.Write(Lane);
            Vehicle.Write(context, fieldContext, bw);
            bw.WriteEnum(Change);
            bw.Write(0);
        }
    }
}