// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:46 AM.

using VaultLib.Core.DB;

namespace VaultLib.Core
{
    /// <summary>
    ///     Provides utilities for the saving process
    /// </summary>
    public class VaultReadContext
    {
        public Database Database { get; }

        public Vault Vault { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultReadContext"/> class.
        /// </summary>
        /// <param name="vault"></param>
        public VaultReadContext(Vault vault)
        {
            Database = vault.Database;
            Vault = vault;
        }
    }
}