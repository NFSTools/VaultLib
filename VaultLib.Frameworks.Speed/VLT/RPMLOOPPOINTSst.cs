using System.Buffers.Binary;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(RPMLOOPPOINTSst))]
public struct RPMLOOPPOINTSst : IComplexType
{
    public int RPM_LD_LOW_PEAK;
    public int RPM_LD_LOW_OUT;
    public int RPM_LD_MED_IN;
    public int RPM_LD_MED_PEAK;
    public int RPM_LD_MED_OUT;
    public int RPM_LD_HI_IN;
    public int RPM_LD_HI_PEAK;
    public int RPM_LD_HI_OUT;
    public int RPM_IDLE_PEAK;
    public int RPM_IDLE_OUT;
    public int RPM_CRZ_LOW_IN;
    public int RPM_CRZ_LOW_PEAK;
    public int RPM_CRZ_LOW_OUT;
    public int RPM_CRZ_MED_IN;
    public int RPM_CRZ_MED_PEAK;
    public int RPM_CRZ_MED_OUT;
    public int RPM_CRZ_HI_IN;
    public int RPM_CRZ_HI_PEAK;
    public int RPM_CRZ_HI_OUT;

    public void EndianSwap()
    {
        RPM_LD_LOW_PEAK = BinaryPrimitives.ReverseEndianness(RPM_LD_LOW_PEAK);
        RPM_LD_LOW_OUT = BinaryPrimitives.ReverseEndianness(RPM_LD_LOW_OUT);
        RPM_LD_MED_IN = BinaryPrimitives.ReverseEndianness(RPM_LD_MED_IN);
        RPM_LD_MED_PEAK = BinaryPrimitives.ReverseEndianness(RPM_LD_MED_PEAK);
        RPM_LD_MED_OUT = BinaryPrimitives.ReverseEndianness(RPM_LD_MED_OUT);
        RPM_LD_HI_IN = BinaryPrimitives.ReverseEndianness(RPM_LD_HI_IN);
        RPM_LD_HI_PEAK = BinaryPrimitives.ReverseEndianness(RPM_LD_HI_PEAK);
        RPM_LD_HI_OUT = BinaryPrimitives.ReverseEndianness(RPM_LD_HI_OUT);
        RPM_IDLE_PEAK = BinaryPrimitives.ReverseEndianness(RPM_IDLE_PEAK);
        RPM_IDLE_OUT = BinaryPrimitives.ReverseEndianness(RPM_IDLE_OUT);
        RPM_CRZ_LOW_IN = BinaryPrimitives.ReverseEndianness(RPM_CRZ_LOW_IN);
        RPM_CRZ_LOW_PEAK = BinaryPrimitives.ReverseEndianness(RPM_CRZ_LOW_PEAK);
        RPM_CRZ_LOW_OUT = BinaryPrimitives.ReverseEndianness(RPM_CRZ_LOW_OUT);
        RPM_CRZ_MED_IN = BinaryPrimitives.ReverseEndianness(RPM_CRZ_MED_IN);
        RPM_CRZ_MED_PEAK = BinaryPrimitives.ReverseEndianness(RPM_CRZ_MED_PEAK);
        RPM_CRZ_MED_OUT = BinaryPrimitives.ReverseEndianness(RPM_CRZ_MED_OUT);
        RPM_CRZ_HI_IN = BinaryPrimitives.ReverseEndianness(RPM_CRZ_HI_IN);
        RPM_CRZ_HI_PEAK = BinaryPrimitives.ReverseEndianness(RPM_CRZ_HI_PEAK);
        RPM_CRZ_HI_OUT = BinaryPrimitives.ReverseEndianness(RPM_CRZ_HI_OUT);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}