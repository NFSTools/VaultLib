using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(CCarDamageThreshold))]
    public class CCarDamageThreshold : VLTBaseType
    {
        public CCarDamageThreshold(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CCarDamageThreshold(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public float Threshold0 { get; set; }
        public float Threshold1 { get; set; }
        public float Threshold2 { get; set; }
        public float Threshold3 { get; set; }
        public float DeltaThreshold0 { get; set; }
        public float DeltaThreshold1 { get; set; }
        public float DeltaThreshold2 { get; set; }
        public float DeltaThreshold3 { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Threshold0 = br.ReadSingle();
            Threshold1 = br.ReadSingle();
            Threshold2 = br.ReadSingle();
            Threshold3 = br.ReadSingle();
            DeltaThreshold0 = br.ReadSingle();
            DeltaThreshold1 = br.ReadSingle();
            DeltaThreshold2 = br.ReadSingle();
            DeltaThreshold3 = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Threshold0);
            bw.Write(Threshold1);
            bw.Write(Threshold2);
            bw.Write(Threshold3);
            bw.Write(DeltaThreshold0);
            bw.Write(DeltaThreshold1);
            bw.Write(DeltaThreshold2);
            bw.Write(DeltaThreshold3);
        }
    }
}