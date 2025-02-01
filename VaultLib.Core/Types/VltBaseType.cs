// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:12 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Types;

public abstract class VltBaseType<TKey> where TKey : struct, IKey<TKey>
{
    public abstract void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br);

    public abstract void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw);
}