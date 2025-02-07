// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 4:00 PM.

using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;

namespace VaultLib.Core;

/// <summary>
///     A vault is the main holder of data. Classes, collections, and collection data are all stored in vaults.
/// </summary>
public class Vault<TKey> where TKey : struct, IKey<TKey>
{
    public Vault(Database<TKey> database, string name)
    {
        Database = database;
        Name = name;
    }

    /// <summary>
    ///     The name of the vault.
    /// </summary>
    public string Name { get; }

    public ulong Version { get; set; }

    /// <summary>
    ///     The database that has this vault
    /// </summary>
    public Database<TKey> Database { get; }

    /// <summary>
    /// This is set to <c>true</c> if this vault is the "primary" vault - the one with class definitions
    /// </summary>
    public bool IsPrimaryVault { get; set; }

    // public VaultSaveContext SaveContext { get; set; }
}