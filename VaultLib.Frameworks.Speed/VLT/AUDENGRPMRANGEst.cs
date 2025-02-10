// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:57 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AUDENGRPMRANGEst))]
public struct AUDENGRPMRANGEst : IComplexType
{
    public int IDLE_RPM;
    public int CRZ_LO_RPM;
    public int CRZ_MED_RPM;
    public int CRZ_HI_RPM;
    public int LD_LOW_RPM;
    public int LD_MED_RPM;
    public int LD_HI_RPM;
    public int REVLMT_RPM;

    public void EndianSwap()
    {
        IDLE_RPM = BinaryPrimitives.ReverseEndianness(IDLE_RPM);
        CRZ_LO_RPM = BinaryPrimitives.ReverseEndianness(CRZ_LO_RPM);
        CRZ_MED_RPM = BinaryPrimitives.ReverseEndianness(CRZ_MED_RPM);
        CRZ_HI_RPM = BinaryPrimitives.ReverseEndianness(CRZ_HI_RPM);
        LD_LOW_RPM = BinaryPrimitives.ReverseEndianness(LD_LOW_RPM);
        LD_MED_RPM = BinaryPrimitives.ReverseEndianness(LD_MED_RPM);
        LD_HI_RPM = BinaryPrimitives.ReverseEndianness(LD_HI_RPM);
        REVLMT_RPM = BinaryPrimitives.ReverseEndianness(REVLMT_RPM);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}