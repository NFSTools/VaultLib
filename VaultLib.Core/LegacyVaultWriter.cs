// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 11:40 AM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core.Chunks;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core
{
    public class LegacyVaultWriter
    {
        private readonly List<BaseExport> _exports = new List<BaseExport>();
        private readonly Vault _vault;

        public LegacyVaultWriter(Vault vault, VaultSaveOptions options = null)
        {
            BinStream = new MemoryStream();
            VltStream = new MemoryStream();

            _vault = vault;
            _vault.SaveContext = new VaultSaveContext(options ?? new VaultSaveOptions())
            {
                Collections = vault.Database.RowManager.GetCollectionsInVault(vault).ToList(),
                Pointers = new HashSet<VltPointer>(VltPointer.FixUpOffsetDestinationTypeComparer),
                Strings = new HashSet<string>(),
                StringOffsets = new Dictionary<string, long>()
            };
        }

        private VaultSaveContext SaveContext => _vault.SaveContext;

        private MemoryStream BinStream { get; }
        private MemoryStream VltStream { get; }

        /// <summary>
        /// Builds data streams and returns them in the form of a tuple.
        /// </summary>
        /// <returns>A tuple with the BIN and VLT streams, in that order.</returns>
        public (MemoryStream bin, MemoryStream vlt) Save()
        {
            BuildExports();
            SaveBin();
            SaveVlt();

            BinStream.Position = 0;
            VltStream.Position = 0;

            return (BinStream, VltStream);
        }

        private void BuildPointers()
        {
            foreach (var pointerObject in _exports.OfType<IPointerObject>()) pointerObject.AddPointers(_vault);
        }

        private void BuildExports()
        {
            if (_vault.IsPrimaryVault)
            {
                _exports.Add(ExportFactory.BuildDatabaseLoad(_vault));

                foreach (var vltClass in _vault.Database.Classes)
                {
                    _exports.Add(ExportFactory.BuildClassLoad(_vault, vltClass));
                    _exports.AddRange(from collection in SaveContext.Collections
                                      where collection.Class.Name == vltClass.Name
                                      select ExportFactory.BuildCollectionLoad(_vault, collection));
                }
            }
            else
            {
                _exports.AddRange(from collection in SaveContext.Collections
                                  select ExportFactory.BuildCollectionLoad(_vault, collection));
            }

            _exports.ForEach(e => e.Prepare(_vault));
        }

        private void SaveBin()
        {
            var bw = new BinaryWriter(BinStream);

            var stringsSet = new HashSet<string>();

            var strings = _vault.SaveContext.Collections.SelectMany(CollectStrings).ToList();
            stringsSet.UnionWith(strings);

            var chunkWriter = new ChunkWriter(bw, _vault);

            var stringsChunk = new BinStringsChunk { Strings = new List<string>(stringsSet) };

            chunkWriter.WriteChunk(stringsChunk);
            var endChunk = new EndChunk();
            chunkWriter.WriteChunk(endChunk);
        }

        private void SaveVlt()
        {
            var bw = new BinaryWriter(VltStream);
            var chunkWriter = new ChunkWriter(bw, _vault);

            {
                var versionChunk = new VltVersionChunk();
                chunkWriter.WriteChunk(versionChunk);

                var startChunk = new VltStartChunk();
                chunkWriter.WriteChunk(startChunk);
            }

            var dependencyChunk = new VltDependencyChunk(new List<string>
            {
                $"{_vault.Name}.vlt",
                $"{_vault.Name}.bin"
            });
            chunkWriter.WriteChunk(dependencyChunk);

            var dataChunk = new VltDataChunk(_exports);
            chunkWriter.WriteChunk(dataChunk);

            var exportChunk = new VltExportChunk(dataChunk.ExportEntries);
            chunkWriter.WriteChunk(exportChunk);
            var binWriter = new BinaryWriter(BinStream);

            foreach (var pointerObject in _exports.OfType<IPointerObject>())
                pointerObject.WritePointerData(_vault, binWriter);

            // after writing exports, we can build pointers
            BuildPointers();

            var pointersChunk = new VltPointersChunk();
            chunkWriter.WriteChunk(pointersChunk);
            var endChunk = new EndChunk();
            chunkWriter.WriteChunk(endChunk);
        }

        private IEnumerable<string> CollectStrings(VltCollection collection)
        {
            return collection.GetData().Values.OfType<IReferencesStrings>().SelectMany(r => r.GetStrings());
        }
    }
}