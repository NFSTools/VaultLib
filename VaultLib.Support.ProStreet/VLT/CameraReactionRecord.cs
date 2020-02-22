using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(CameraReactionRecord))]
    public class CameraReactionRecord : VLTBaseType
    {
        public CameraReactionRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CameraReactionRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public CameraReactionType Type { get; set; }
        public float InputMin { get; set; }
        public float[] ValueMin { get; set; }
        public float InputMax { get; set; }
        public float[] ValueMax { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Type = br.ReadEnum<CameraReactionType>();
            InputMin = br.ReadSingle();
            ValueMin = br.ReadArray(br.ReadSingle, 2);
            InputMax = br.ReadSingle();
            ValueMax = br.ReadArray(br.ReadSingle, 2);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(Type);
            bw.Write(InputMin);
            bw.WriteArray(ValueMin, bw.Write);
            bw.Write(InputMax);
            bw.WriteArray(ValueMax, bw.Write);
        }
    }
}