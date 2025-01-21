// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:56 PM.

using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AUDENGLOOPVOLUMESst))]
public struct AUDENGLOOPVOLUMESst
{
    public int IDLE_VOL;
    public int CRZ_LOW_VOL;
    public int CRZ_MED_VOL;
    public int CRZ_HI_VOL;
    public int LD_LOW_VOL;
    public int LD_MED_VOL;
    public int LD_HI_VOL;
    public int REVLMT_VOL;
}