// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:57 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AUDENGRPMRANGEst))]
public struct AUDENGRPMRANGEst
{
    public int IDLE_RPM;
    public int CRZ_LO_RPM;
    public int CRZ_MED_RPM;
    public int CRZ_HI_RPM;
    public int LD_LOW_RPM;
    public int LD_MED_RPM;
    public int LD_HI_RPM;
    public int REVLMT_RPM;
}