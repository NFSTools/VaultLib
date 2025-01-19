using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CCarDamageLevels))]
    public struct CCarDamageLevels
    {
        public float Speed0;
        public float Speed1;
        public float Speed2;
        public float Influence;
        public float LightCrack;
        public float LightShatter;
        public float WindowCrack;
        public float WindowShatter;
    }
}