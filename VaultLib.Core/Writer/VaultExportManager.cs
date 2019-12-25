using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VaultLib.Core.Exports;

namespace VaultLib.Core.Writer
{
    /// <summary>
    /// Manages information about exports to be built into a file.
    /// </summary>
    public class VaultExportManager
    {
        private Vault Vault { get; }
        private List<BaseExport> Exports { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultExportManager"/> class.
        /// </summary>
        /// <param name="vault">The vault to build exports for.</param>
        public VaultExportManager(Vault vault)
        {
            Vault = vault;
            Exports = new List<BaseExport>();
        }

        /// <summary>
        /// Builds exports for the vault.
        /// </summary>
        /// <remarks>This resets the list of exports.</remarks>
        public void BuildVaultExports()
        {
            Exports.Clear();

            if (Vault.IsPrimaryVault)
            {
                Exports.Add(ExportFactory.BuildDatabaseLoad(Vault));

                foreach (var vltClass in Vault.Database.Classes)
                {
                    Exports.Add(ExportFactory.BuildClassLoad(Vault, vltClass));
                    Exports.AddRange(from collection in Vault.SaveContext.Collections
                                     where collection.Class.Name == vltClass.Name
                                     select ExportFactory.BuildCollectionLoad(Vault, collection));
                }
            }
            else
            {
                Exports.AddRange(from collection in Vault.SaveContext.Collections
                                 select ExportFactory.BuildCollectionLoad(Vault, collection));
            }
        }

        /// <summary>
        /// Performs preparation work on each export.
        /// </summary>
        public void PrepareExports()
        {
            Exports.ForEach(e => e.Prepare(Vault));
        }

        /// <summary>
        /// Adds an export to the list of exports.
        /// </summary>
        /// <param name="export">The export to add.</param>
        public void AddExport(BaseExport export)
        {
            Exports.Add(export);
        }

        /// <summary>
        /// Gets a read-only view of the list of exports.
        /// </summary>
        /// <returns>The read-only list of exports.</returns>
        public IList<BaseExport> GetExports()
        {
            return new ReadOnlyCollection<BaseExport>(Exports);
        }
    }
}