// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 4:00 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;

namespace VaultLib.Core
{
    /// <summary>
    /// A vault is the main holder of data. Classes, collections, and collection data are all stored in vaults.
    /// </summary>
    public class Vault
    {
        /// <summary>
        /// The name of the vault.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The BIN data stream, where most of the actual data lies.
        /// </summary>
        public Stream BinStream { get; set; }

        /// <summary>
        /// The VLT data stream, where most of the information lies. (Some is in BIN. Why?!)
        /// </summary>
        public Stream VltStream { get; set; }

        /// <summary>
        /// The exported data items.
        /// </summary>
        public List<BaseExport> Exports { get; }

        /// <summary>
        /// The data pointers.
        /// </summary>
        public List<VLTPointer> Pointers { get; }

        /// <summary>
        /// The database that has this vault
        /// </summary>
        public Database Database { get; set; }

        public VaultSaveContext SaveContext { get; set; }

        public ByteOrder ByteOrder { get; set; }

        public Vault(string name)
        {
            this.Name = name;
            this.Exports = new List<BaseExport>();
            this.Pointers = new List<VLTPointer>();
        }
    }
}