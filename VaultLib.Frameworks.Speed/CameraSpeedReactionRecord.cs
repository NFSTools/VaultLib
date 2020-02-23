using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(CameraSpeedReactionRecord))]
    public class CameraSpeedReactionRecord : VLTBaseType
    {
        public CameraSpeedReactionRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CameraSpeedReactionRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float SpeedMin { get; set; }
        public float ValueMin { get; set; }
        public float SpeedMax { get; set; }
        public float ValueMax { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            SpeedMin = br.ReadSingle();
            ValueMin = br.ReadSingle();
            SpeedMax = br.ReadSingle();
            ValueMax = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(SpeedMin);
            bw.Write(ValueMin);
            bw.Write(SpeedMax);
            bw.Write(ValueMax);
        }
    }
}