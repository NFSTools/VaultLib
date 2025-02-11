// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/06/2019 @ 9:00 PM.

using System.Runtime.InteropServices;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT.Sound;

[VltTypeInfo("Sound::BinarySequence")]
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
public struct BinarySequence : IComplexType
{
    [FieldOffset(0)] [MarshalAs(UnmanagedType.U1)]
    public bool Value;

    [FieldOffset(4)] public float Duration;

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}