using System.Buffers.Binary;
using System.Runtime.InteropServices;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT.Physics;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(PerformanceWeightWithDirection))]
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 12)]
public struct PerformanceWeightWithDirection : IComplexType
{
    [FieldOffset(0)] public ePerformanceType mPerformanceType;

    [FieldOffset(4)] [MarshalAs(UnmanagedType.U1)]
    public bool mInverse;

    [FieldOffset(8)] public float mPercentage;

    public void EndianSwap()
    {
        mPerformanceType = (ePerformanceType)BinaryPrimitives.ReverseEndianness((uint)mPerformanceType);
        mPercentage = mPercentage.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}