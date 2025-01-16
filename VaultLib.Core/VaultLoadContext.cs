// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:46 AM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VLT32Hasher = VaultLib.Core.Hashing.VLT32Hasher;
using VLT64Hasher = VaultLib.Core.Hashing.VLT64Hasher;

namespace VaultLib.Core
{
    /// <summary>
    ///     Provides utilities for the saving process
    /// </summary>
    public class VaultLoadContext
    {
        public Database Database { get; }

        public Vault Vault { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultLoadContext"/> class.
        /// </summary>
        /// <param name="vault"></param>
        public VaultLoadContext(Vault vault)
        {
            Database = vault.Database;
            Vault = vault;
        }
    }
}