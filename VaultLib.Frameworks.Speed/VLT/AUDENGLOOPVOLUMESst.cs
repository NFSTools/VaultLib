// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 3:56 PM.

using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(AUDENGLOOPVOLUMESst))]
public struct AUDENGLOOPVOLUMESst : IComplexType
{
    public int IDLE_VOL;
    public int CRZ_LOW_VOL;
    public int CRZ_MED_VOL;
    public int CRZ_HI_VOL;
    public int LD_LOW_VOL;
    public int LD_MED_VOL;
    public int LD_HI_VOL;
    public int REVLMT_VOL;

    public void EndianSwap()
    {
        IDLE_VOL = BinaryPrimitives.ReverseEndianness(IDLE_VOL);
        CRZ_LOW_VOL = BinaryPrimitives.ReverseEndianness(CRZ_LOW_VOL);
        CRZ_MED_VOL = BinaryPrimitives.ReverseEndianness(CRZ_MED_VOL);
        CRZ_HI_VOL = BinaryPrimitives.ReverseEndianness(CRZ_HI_VOL);
        LD_LOW_VOL = BinaryPrimitives.ReverseEndianness(LD_LOW_VOL);
        LD_MED_VOL = BinaryPrimitives.ReverseEndianness(LD_MED_VOL);
        LD_HI_VOL = BinaryPrimitives.ReverseEndianness(LD_HI_VOL);
        REVLMT_VOL = BinaryPrimitives.ReverseEndianness(REVLMT_VOL);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}