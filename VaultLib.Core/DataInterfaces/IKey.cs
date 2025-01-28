using System.IO;
using System.Numerics;
using VaultLib.Core.Hashing;

namespace VaultLib.Core.DataInterfaces;

public interface IKey<TSelf> : IEqualityOperators<TSelf, TSelf, bool> where TSelf : IKey<TSelf>
{
    static abstract TSelf Zero { get; }

    static abstract TSelf FromString(string value);

    static abstract TSelf Read(BinaryReader reader);

    void Write(BinaryWriter writer);
}

public readonly record struct Key32(uint Hash) : IKey<Key32>
{
    public static Key32 Zero => default;

    public static Key32 FromString(string value)
    {
        return new Key32(Vlt32Hasher.Hash(value));
    }

    public static Key32 Read(BinaryReader reader)
    {
        return new Key32(reader.ReadUInt32());
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Hash);
    }

    public override string ToString()
    {
        return $"0x{Hash:X8}";
    }
}

public readonly record struct Key64(ulong Hash) : IKey<Key64>
{
    public static Key64 Zero => default;

    public static Key64 FromString(string value)
    {
        return new Key64(Vlt64Hasher.Hash(value));
    }

    public static Key64 Read(BinaryReader reader)
    {
        return new Key64(reader.ReadUInt64());
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Hash);
    }

    public override string ToString()
    {
        return $"0x{Hash:X16}";
    }
}