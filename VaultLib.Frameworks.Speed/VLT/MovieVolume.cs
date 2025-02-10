using System.Buffers.Binary;
using System.Runtime.InteropServices;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(MovieVolume))]
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
public struct MovieVolume : IComplexType
{
    [FieldOffset(0)] public uint Hash;
    [FieldOffset(4)] public byte Volume;

    public void EndianSwap()
    {
        Hash = BinaryPrimitives.ReverseEndianness(Hash);
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}