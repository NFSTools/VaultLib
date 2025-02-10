using System.Buffers.Binary;
using System.Runtime.InteropServices;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(SteeringSensitivityParameter))]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct SteeringSensitivityParameter : IComplexType
{
    public eSteeringCurveStyle CurveStyle;
    public float CurvePower;
    public float CurveMultiplierLowSpeed;
    public float CurveMultiplierHighSpeed;
    public float InnerDeadZone;
    public float OuterDeadZone;
    public ushort NumberOfSteps;

    public void EndianSwap()
    {
        CurveStyle = (eSteeringCurveStyle)BinaryPrimitives.ReverseEndianness((uint)CurveStyle);
        CurvePower = CurvePower.EndianSwap();
        CurveMultiplierLowSpeed = CurveMultiplierLowSpeed.EndianSwap();
        CurveMultiplierHighSpeed = CurveMultiplierHighSpeed.EndianSwap();
        InnerDeadZone = InnerDeadZone.EndianSwap();
        OuterDeadZone = OuterDeadZone.EndianSwap();
        NumberOfSteps = BinaryPrimitives.ReverseEndianness(NumberOfSteps);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}