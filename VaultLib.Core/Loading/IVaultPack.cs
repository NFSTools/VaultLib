// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:54 PM.

using System.IO;
using VaultLib.Core.DB;

namespace VaultLib.Core.Loading
{
    /// <summary>
    /// Reads/writes vaults from/to a stream.
    /// </summary>
    public interface IVaultPack
    {
        /// <summary>
        /// Reads vaults from the given binary stream and loads them into the given database.
        /// </summary>
        /// <param name="br">The <see cref="BinaryReader"/> instance to read data from</param>
        /// <param name="database">The <see cref="Database"/> that vaults will be loaded in to</param>
        /// <param name="loadingOptions">The <see cref="PackLoadingOptions"/> instance being used</param>
        void Load(BinaryReader br, Database database, PackLoadingOptions loadingOptions = null);
    }
}