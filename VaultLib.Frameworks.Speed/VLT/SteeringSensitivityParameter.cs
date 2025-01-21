using System.Runtime.InteropServices;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(SteeringSensitivityParameter))]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct SteeringSensitivityParameter
{
    public eSteeringCurveStyle CurveStyle;
    public float CurvePower;
    public float CurveMultiplierLowSpeed;
    public float CurveMultiplierHighSpeed;
    public float InnerDeadZone;
    public float OuterDeadZone;
    public ushort NumberOfSteps;
}