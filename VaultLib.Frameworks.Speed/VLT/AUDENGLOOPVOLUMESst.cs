// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:56 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(AUDENGLOOPVOLUMESst))]
    public class AUDENGLOOPVOLUMESst : VltBaseType
    {
        public int IDLE_VOL { get; set; }
        public int CRZ_LOW_VOL { get; set; }
        public int CRZ_MED_VOL { get; set; }
        public int CRZ_HI_VOL { get; set; }
        public int LD_LOW_VOL { get; set; }
        public int LD_MED_VOL { get; set; }
        public int LD_HI_VOL { get; set; }
        public int REVLMT_VOL { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            IDLE_VOL = br.ReadInt32();
            CRZ_LOW_VOL = br.ReadInt32();
            CRZ_MED_VOL = br.ReadInt32();
            CRZ_HI_VOL = br.ReadInt32();
            LD_LOW_VOL = br.ReadInt32();
            LD_MED_VOL = br.ReadInt32();
            LD_HI_VOL = br.ReadInt32();
            REVLMT_VOL = br.ReadInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(IDLE_VOL);
            bw.Write(CRZ_LOW_VOL);
            bw.Write(CRZ_MED_VOL);
            bw.Write(CRZ_HI_VOL);
            bw.Write(LD_LOW_VOL);
            bw.Write(LD_MED_VOL);
            bw.Write(LD_HI_VOL);
            bw.Write(REVLMT_VOL);
        }
    }
}