using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(RPMLOOPPOINTSst))]
    public class RPMLOOPPOINTSst : VLTBaseType
    {
        public int RPM_LD_LOW_PEAK { get; set; }
        public int RPM_LD_LOW_OUT { get; set; }
        public int RPM_LD_MED_IN { get; set; }
        public int RPM_LD_MED_PEAK { get; set; }
        public int RPM_LD_MED_OUT { get; set; }
        public int RPM_LD_HI_IN { get; set; }
        public int RPM_LD_HI_PEAK { get; set; }
        public int RPM_LD_HI_OUT { get; set; }
        public int RPM_IDLE_PEAK { get; set; }
        public int RPM_IDLE_OUT { get; set; }
        public int RPM_CRZ_LOW_IN { get; set; }
        public int RPM_CRZ_LOW_PEAK { get; set; }
        public int RPM_CRZ_LOW_OUT { get; set; }
        public int RPM_CRZ_MED_IN { get; set; }
        public int RPM_CRZ_MED_PEAK { get; set; }
        public int RPM_CRZ_MED_OUT { get; set; }
        public int RPM_CRZ_HI_IN { get; set; }
        public int RPM_CRZ_HI_PEAK { get; set; }
        public int RPM_CRZ_HI_OUT { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            RPM_LD_LOW_PEAK = br.ReadInt32();
            RPM_LD_LOW_OUT = br.ReadInt32();
            RPM_LD_MED_IN = br.ReadInt32();
            RPM_LD_MED_PEAK = br.ReadInt32();
            RPM_LD_MED_OUT = br.ReadInt32();
            RPM_LD_HI_IN = br.ReadInt32();
            RPM_LD_HI_PEAK = br.ReadInt32();
            RPM_LD_HI_OUT = br.ReadInt32();
            RPM_IDLE_PEAK = br.ReadInt32();
            RPM_IDLE_OUT = br.ReadInt32();
            RPM_CRZ_LOW_IN = br.ReadInt32();
            RPM_CRZ_LOW_PEAK = br.ReadInt32();
            RPM_CRZ_LOW_OUT = br.ReadInt32();
            RPM_CRZ_MED_IN = br.ReadInt32();
            RPM_CRZ_MED_PEAK = br.ReadInt32();
            RPM_CRZ_MED_OUT = br.ReadInt32();
            RPM_CRZ_HI_IN = br.ReadInt32();
            RPM_CRZ_HI_PEAK = br.ReadInt32();
            RPM_CRZ_HI_OUT = br.ReadInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(RPM_LD_LOW_PEAK);
            bw.Write(RPM_LD_LOW_OUT);
            bw.Write(RPM_LD_MED_IN);
            bw.Write(RPM_LD_MED_PEAK);
            bw.Write(RPM_LD_MED_OUT);
            bw.Write(RPM_LD_HI_IN);
            bw.Write(RPM_LD_HI_PEAK);
            bw.Write(RPM_LD_HI_OUT);
            bw.Write(RPM_IDLE_PEAK);
            bw.Write(RPM_IDLE_OUT);
            bw.Write(RPM_CRZ_LOW_IN);
            bw.Write(RPM_CRZ_LOW_PEAK);
            bw.Write(RPM_CRZ_LOW_OUT);
            bw.Write(RPM_CRZ_MED_IN);
            bw.Write(RPM_CRZ_MED_PEAK);
            bw.Write(RPM_CRZ_MED_OUT);
            bw.Write(RPM_CRZ_HI_IN);
            bw.Write(RPM_CRZ_HI_PEAK);
            bw.Write(RPM_CRZ_HI_OUT);
        }

        public RPMLOOPPOINTSst(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public RPMLOOPPOINTSst(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}