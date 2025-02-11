using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(CameraSpeedReactionRecord))]
public struct CameraSpeedReactionRecord : IComplexType
{
    public float SpeedMin;
    public float ValueMin;
    public float SpeedMax;
    public float ValueMax;

    public void EndianSwap()
    {
        SpeedMin = SpeedMin.EndianSwap();
        ValueMin = ValueMin.EndianSwap();
        SpeedMax = SpeedMax.EndianSwap();
        ValueMax = ValueMax.EndianSwap();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}