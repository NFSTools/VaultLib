// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 9:27 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Utils;

/// <summary>
///     Generic interface for reading and writing binary structures, with access to the containing <see cref="Vault{TKey}"/>.
/// </summary>
public interface IVaultFileAccess<TKey> where TKey : struct, IKey<TKey>
{
    void Read(VaultReadContext<TKey> context, BinaryReader br);
    void Write(VaultWriteContext<TKey> context, BinaryWriter bw);
}