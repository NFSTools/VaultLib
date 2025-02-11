using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.GameCore;

[VltTypeInfo("GameCore::PhysicsEntityPair")]
public struct PhysicsEntityPair : IComplexType
{
    public PhysicsEntity Entity1;
    public PhysicsEntity Entity2;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}