using System.Numerics;
using VaultLib.Core.Hashing;

namespace VaultLib.Core.DataInterfaces;

public interface IKey<TSelf> : IEqualityOperators<TSelf, TSelf, bool> where TSelf : IKey<TSelf>
{
    static abstract TSelf FromString(string value);
}

public struct Key32(uint hash) : IKey<Key32>
{
    public uint Hash = hash;

    public static Key32 FromString(string value)
    {
        return new Key32(Vlt32Hasher.Hash(value));
    }

    public static bool operator ==(Key32 left, Key32 right)
    {
        return left.Hash == right.Hash;
    }

    public static bool operator !=(Key32 left, Key32 right)
    {
        return left.Hash != right.Hash;
    }
}

public struct Key64(ulong hash) : IKey<Key64>
{
    public ulong Hash = hash;
    
    public static Key64 FromString(string value)
    {
        return new Key64(Vlt64Hasher.Hash(value));
    }

    public static bool operator ==(Key64 left, Key64 right)
    {
        return left.Hash == right.Hash;
    }

    public static bool operator !=(Key64 left, Key64 right)
    {
        return left.Hash != right.Hash;
    }
}