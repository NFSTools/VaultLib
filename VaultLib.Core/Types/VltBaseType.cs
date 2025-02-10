// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:12 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Core.Types;

public abstract class VltBaseType<TKey> : IComplexType where TKey : struct, IKey<TKey>
{
    public abstract void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br);

    public abstract void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw);

    public void EndianSwap()
    {
        throw new System.NotImplementedException();
    }

    public abstract object Clone();
}