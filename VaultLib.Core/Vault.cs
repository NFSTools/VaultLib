// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 4:00 PM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;

namespace VaultLib.Core;

/// <summary>
///     A vault is the main holder of data. Classes, collections, and collection data are all stored in vaults.
/// </summary>
public class Vault
{
    public Vault(string name)
    {
        Name = name;
        Exports = new List<BaseExport>();
    }

    /// <summary>
    ///     The name of the vault.
    /// </summary>
    public string Name { get; }
        
    public ulong Version { get; set; }

    /// <summary>
    ///     The exported data items.
    /// </summary>
    public List<BaseExport> Exports { get; }

    /// <summary>
    ///     The database that has this vault
    /// </summary>
    public Database Database { get; set; }

    /// <summary>
    /// This is set to <c>true</c> if this vault is the "primary" vault - the one with class definitions
    /// </summary>
    public bool IsPrimaryVault { get; set; }

    // public VaultSaveContext SaveContext { get; set; }

    public ByteOrder ByteOrder { get; set; }
}