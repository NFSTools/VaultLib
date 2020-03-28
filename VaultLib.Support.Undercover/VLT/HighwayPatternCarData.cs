using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(HighwayPatternCarData))]
    public class HighwayPatternCarData : VLTBaseType
    {
        public HighwayPatternCarData(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Vehicle = new RefSpec(Class, Field, Collection);
        }

        public int Row { get; set; }
        public int Lane { get; set; }
        public RefSpec Vehicle { get; set; }
        public EAILaneChangeType Change { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Row = br.ReadInt32();
            Lane = br.ReadInt32();
            Vehicle.Read(vault, br);
            Change = br.ReadEnum<EAILaneChangeType>();

            var v = br.ReadUInt32();
            if (v != 0)
                throw new InvalidDataException();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Row);
            bw.Write(Lane);
            Vehicle.Write(vault, bw);
            bw.WriteEnum(Change);
            bw.Write(0);
        }
    }
}