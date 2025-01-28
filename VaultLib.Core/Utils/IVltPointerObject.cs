// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 10:51 AM.

using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Utils;

public interface IVltPointerObject<TKey> where TKey : struct, IKey<TKey>
{
    /// <summary>
    ///     Read data stored through pointers to the BIN stream
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fieldContext"></param>
    /// <param name="br"></param>
    void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br);

    /// <summary>
    ///     Read data stored through pointers to the BIN stream
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fieldContext"></param>
    /// <param name="bw"></param>
    void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryWriter bw);

    /// <summary>
    ///     Add pointer information to the vault
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fieldContext"></param>
    void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext);
}