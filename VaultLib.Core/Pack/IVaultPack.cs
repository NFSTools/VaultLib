// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:54 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;

namespace VaultLib.Core.Pack;

/// <summary>
///     Reads/writes vaults from/to a stream.
/// </summary>
public interface IVaultPack
{
    /// <summary>
    ///     Reads vaults from the given binary stream and loads them into the given database.
    /// </summary>
    /// <param name="br">The <see cref="BinaryReader" /> instance to read data from</param>
    /// <param name="database">The <see cref="Database{TKey}" /> that vaults will be loaded in to</param>
    /// <param name="loadingOptions">The options for the loading process</param>
    IList<Vault<TKey>> Load<TKey>(BinaryReader br, Database<TKey> database, PackLoadingOptions? loadingOptions = null)
        where TKey : struct, IKey<TKey>;

    /// <summary>
    /// Saves the given vaults to the given binary stream.
    /// </summary>
    /// <param name="bw">The <see cref="BinaryWriter"/> to write data to.</param>
    /// <param name="vaults">The list of <see cref="Vault{TKey}"/> instances to save/</param>
    /// <param name="savingOptions">The options for the saving process</param>
    void Save<TKey>(BinaryWriter bw, IList<Vault<TKey>> vaults, PackSavingOptions? savingOptions = null)
        where TKey : struct, IKey<TKey>;
}