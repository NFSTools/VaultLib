using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(StitchCollisionVol))]
    public struct StitchCollisionVol
    {
        public short Vol1;
        public short Vol2;
        public short Vol3;
        public short Vol4;
    }
}