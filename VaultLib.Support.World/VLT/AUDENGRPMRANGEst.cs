// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:57 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(AUDENGRPMRANGEst))]
    public class AUDENGRPMRANGEst : VLTBaseType
    {
        public int IDLE_RPM { get; set; }
        public int CRZ_LO_RPM { get; set; }
        public int CRZ_MED_RPM { get; set; }
        public int CRZ_HI_RPM { get; set; }
        public int LD_LOW_RPM { get; set; }
        public int LD_MED_RPM { get; set; }
        public int LD_HI_RPM { get; set; }
        public int REVLMT_RPM { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            IDLE_RPM = br.ReadInt32();
            CRZ_LO_RPM = br.ReadInt32();
            CRZ_MED_RPM = br.ReadInt32();
            CRZ_HI_RPM = br.ReadInt32();
            LD_LOW_RPM = br.ReadInt32();
            LD_MED_RPM = br.ReadInt32();
            LD_HI_RPM = br.ReadInt32();
            REVLMT_RPM = br.ReadInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(IDLE_RPM);
            bw.Write(CRZ_LO_RPM);
            bw.Write(CRZ_MED_RPM);
            bw.Write(CRZ_HI_RPM);
            bw.Write(LD_LOW_RPM);
            bw.Write(LD_MED_RPM);
            bw.Write(LD_HI_RPM);
            bw.Write(REVLMT_RPM);
        }

        public AUDENGRPMRANGEst(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public AUDENGRPMRANGEst(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}