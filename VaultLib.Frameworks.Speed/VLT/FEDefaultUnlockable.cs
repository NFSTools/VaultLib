using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(FEDefaultUnlockable))]
    public struct FEDefaultUnlockable
    {
        public eUnlockableEntity UnlockType;
        public uint UnlockName;
        public int UnlockLevel;
        public int UnlockTier;
    }
}