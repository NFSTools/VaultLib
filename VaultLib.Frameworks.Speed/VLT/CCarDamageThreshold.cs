using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CCarDamageThreshold))]
    public struct CCarDamageThreshold
    {
        public float Threshold0 { get; set; }
        public float Threshold1 { get; set; }
        public float Threshold2 { get; set; }
        public float Threshold3 { get; set; }
        public float DeltaThreshold0 { get; set; }
        public float DeltaThreshold1 { get; set; }
        public float DeltaThreshold2 { get; set; }
        public float DeltaThreshold3 { get; set; }
    }
}