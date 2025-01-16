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
        private VaultSaveContext SaveContext { get; }
        private List<BaseExport> Exports { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultExportManager"/> class.
        /// </summary>
        /// <param name="saveContext">The vault to build exports for.</param>
        public VaultExportManager(VaultSaveContext saveContext)
        {
            SaveContext = saveContext;
            Exports = new List<BaseExport>();
        }

        /// <summary>
        /// Builds exports for the vault.
        /// </summary>
        /// <remarks>This resets the list of exports.</remarks>
        public void BuildVaultExports()
        {
            Exports.Clear();

            var exportFactory = SaveContext.Database.ExportFactory;
            
            if (SaveContext.Vault.IsPrimaryVault)
            {
                Exports.Add(exportFactory.BuildDatabaseLoad());

                foreach (var vltClass in SaveContext.Database.Classes)
                {
                    Exports.Add(exportFactory.BuildClassLoad(vltClass));
                    Exports.AddRange(from collection in SaveContext.Collections
                        where collection.Class.Name == vltClass.Name
                        select exportFactory.BuildCollectionLoad(collection));
                }
            }
            else
            {
                Exports.AddRange(from collection in SaveContext.Collections
                    select exportFactory.BuildCollectionLoad(collection));
            }
        }

        /// <summary>
        /// Performs preparation work on each export.
        /// </summary>
        public void PrepareExports()
        {
            Exports.ForEach(e => e.Prepare(SaveContext.Vault));
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