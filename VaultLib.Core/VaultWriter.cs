using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core.Chunks;
using VaultLib.Core.Data;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;
using VaultLib.Core.Writer;

namespace VaultLib.Core
{
    /// <summary>
    /// Generates BIN and VLT data streams for a <see cref="VaultLib.Core.Vault"/> instance.
    /// </summary>
    public class VaultWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VaultWriter"/> class.
        /// </summary>
        /// <param name="vault">The <see cref="VaultLib.Core.Vault"/> instance to generate data for.</param>
        /// <param name="options">The options for the saving process.</param>
        public VaultWriter(Vault vault, VaultSaveOptions options)
        {
            Vault = vault;
            Options = options;
            ExportManager = new VaultExportManager(vault);

            Vault.SaveContext = new VaultSaveContext(options)
            {
                Collections = vault.Database.RowManager.GetCollectionsInVault(vault).ToList(),
                Pointers = new HashSet<VltPointer>(VltPointer.FixUpOffsetDestinationTypeComparer),
                Strings = new HashSet<string>(),
                StringOffsets = new Dictionary<string, long>()
            };

            ExportManager.BuildVaultExports();
        }

        /// <summary>
        /// Gets the vault to generate data for.
        /// </summary>
        public Vault Vault { get; }

        /// <summary>
        /// Gets the options for the saving process. 
        /// </summary>
        public VaultSaveOptions Options { get; }

        /// <summary>
        /// Gets the export manager.
        /// </summary>
        public VaultExportManager ExportManager { get; }

        /// <summary>
        /// Builds BIN and VLT streams for the vault and returns them.
        /// </summary>
        /// <returns>The generated streams.</returns>
        public VaultStreamInfo BuildVault()
        {
            ExportManager.PrepareExports();

            BinStream = BuildBinStream();
            VltStream = BuildVltStream();

            BinStream.Position = VltStream.Position = 0;

            return new VaultStreamInfo(BinStream, VltStream);
        }

        #region Internal Implementation

        private Stream BinStream { get; set; }
        private Stream VltStream { get; set; }

        private Stream BuildBinStream()
        {
            MemoryStream ms = new MemoryStream(8192);
            BinaryWriter bw = new BinaryWriter(ms);

            ChunkWriter cw = new ChunkWriter(bw, Vault);
            var stringsSet = new HashSet<string>();

            var strings = Vault.SaveContext.Collections.SelectMany(CollectStrings).ToList();
            stringsSet.UnionWith(strings);
            var stringsChunk = new BinStringsChunk { Strings = new List<string>(stringsSet) };

            cw.WriteChunk(stringsChunk);
            cw.WriteChunk(new EndChunk());

            return ms;
        }

        private Stream BuildVltStream()
        {
            MemoryStream ms = new MemoryStream(8192);
            BinaryWriter bw = new BinaryWriter(ms);
            ChunkWriter cw = new ChunkWriter(bw, Vault);

            var versionChunk = new VltVersionChunk();
            cw.WriteChunk(versionChunk);

            var startChunk = new VltStartChunk();
            cw.WriteChunk(startChunk);

            var dependencyChunk = new VltDependencyChunk(new List<string>
            {
                $"{Vault.Name}.vlt",
                $"{Vault.Name}.bin"
            });
            cw.WriteChunk(dependencyChunk);

            var dataChunk = new VltDataChunk(ExportManager.GetExports());
            cw.WriteChunk(dataChunk);

            var exportChunk = new VltExportChunk(dataChunk.ExportEntries);
            cw.WriteChunk(exportChunk);
            var binWriter = new BinaryWriter(BinStream);

            foreach (var pointerObject in ExportManager.GetExports().OfType<IPointerObject>())
                pointerObject.WritePointerData(Vault, binWriter);

            // after writing exports, we can build pointers
            BuildPointers();

            var pointersChunk = new VltPointersChunk();
            cw.WriteChunk(pointersChunk);
            var endChunk = new EndChunk();
            cw.WriteChunk(endChunk);

            return ms;
        }

        private void BuildPointers()
        {
            foreach (var pointerObject in ExportManager.GetExports().OfType<IPointerObject>()) 
                pointerObject.AddPointers(Vault);
        }

        private static IEnumerable<string> CollectStrings(VltCollection collection)
        {
            return collection.GetData().Values.OfType<IReferencesStrings>().SelectMany(r => r.GetStrings());
        }

        #endregion
    }
}