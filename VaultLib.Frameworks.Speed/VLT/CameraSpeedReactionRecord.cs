using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CameraSpeedReactionRecord))]
    public struct CameraSpeedReactionRecord
    {
        public float SpeedMin;
        public float ValueMin;
        public float SpeedMax;
        public float ValueMax;
    }
}