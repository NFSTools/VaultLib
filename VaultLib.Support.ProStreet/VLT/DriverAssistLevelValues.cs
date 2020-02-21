using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(DriverAssistLevelValues))]
    public class DriverAssistLevelValues : VLTBaseType
    {
        public DriverAssistLevelValues(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public DriverAssistLevelValues(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public int TractionControlLevel { get; set; }
        public int AntilockBrakeLevel { get; set; }
        public int StabilityControlLevel { get; set; }
        public int RaceLineAssist { get; set; }
        public int BrakingAssist { get; set; }
        public int DriftAssist { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            TractionControlLevel = br.ReadInt32();
            AntilockBrakeLevel = br.ReadInt32();
            StabilityControlLevel = br.ReadInt32();
            RaceLineAssist = br.ReadInt32();
            BrakingAssist = br.ReadInt32();
            DriftAssist = br.ReadInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(TractionControlLevel);
            bw.Write(AntilockBrakeLevel);
            bw.Write(StabilityControlLevel);
            bw.Write(RaceLineAssist);
            bw.Write(BrakingAssist);
            bw.Write(DriftAssist);
        }
    }
}