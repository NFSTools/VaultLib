// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 11:40 AM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core.Chunks;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core
{
    public class VaultWriter
    {
        private readonly List<BaseExport> _exports = new List<BaseExport>();
        private readonly Vault _vault;

        public VaultWriter(List<VLTCollection> allCollections, Vault vault)
        {
            BinStream = new MemoryStream();
            VltStream = new MemoryStream();

            _vault = vault;
            _vault.SaveContext = new VaultSaveContext
            {
                Collections = allCollections.FindAll(c => c.Vault.Name == _vault.Name),
                Pointers = new HashSet<VLTPointer>(VLTPointer.FixUpOffsetDestinationTypeComparer),
                Strings = new HashSet<string>(),
                StringOffsets = new Dictionary<string, long>()
            };
        }

        private VaultSaveContext SaveContext => _vault.SaveContext;

        private MemoryStream BinStream { get; }
        private MemoryStream VltStream { get; }

        public void Save()
        {
            BuildExports();
            SaveBIN();
            SaveVLT();

            BinStream.Position = 0;
            VltStream.Position = 0;

            _vault.BinStream = BinStream;
            _vault.VltStream = VltStream;
        }

        private void BuildPointers()
        {
            foreach (var pointerObject in _exports.OfType<IPointerObject>()) pointerObject.AddPointers(_vault);
        }

        private void BuildExports()
        {
            if (_vault.Name == "db")
            {
                _exports.Add(ExportFactory.BuildDatabaseLoad(_vault));

                foreach (var vltClass in _vault.Database.Classes)
                {
                    _exports.Add(ExportFactory.BuildClassLoad(_vault, vltClass));
                    _exports.AddRange(from collection in SaveContext.Collections
                        where collection.ClassName == vltClass.Name
                        orderby collection.Level
                        select ExportFactory.BuildCollectionLoad(_vault, collection));
                }
            }
            else
            {
                _exports.AddRange(from collection in SaveContext.Collections
                    select ExportFactory.BuildCollectionLoad(_vault, collection));
            }

            _exports.ForEach(e => e.Prepare());
        }

        private void SaveBIN()
        {
            var bw = new BinaryWriter(BinStream);

            var stringsSet = new HashSet<string>();

            var strings = _vault.SaveContext.Collections.SelectMany(CollectStrings).ToList();
            stringsSet.UnionWith(strings);

            var chunkWriter = new ChunkWriter(bw, _vault);

            var stringsChunk = new BINStringsChunk();
            stringsChunk.Strings = new List<string>(stringsSet);

            chunkWriter.WriteChunk(stringsChunk);
            var endChunk = new EndChunk();
            chunkWriter.WriteChunk(endChunk);
        }

        private void SaveVLT()
        {
            var bw = new BinaryWriter(VltStream);
            var chunkWriter = new ChunkWriter(bw, _vault);

            {
                var versionChunk = new VLTVersionChunk();
                chunkWriter.WriteChunk(versionChunk);

                var startChunk = new VLTStartChunk();
                chunkWriter.WriteChunk(startChunk);
            }

            var dependencyChunk = new VLTDependencyChunk(new List<string>
            {
                $"{_vault.Name}.vlt",
                $"{_vault.Name}.bin"
            });
            chunkWriter.WriteChunk(dependencyChunk);

            var dataChunk = new VLTDataChunk(_exports);
            chunkWriter.WriteChunk(dataChunk);

            var exportChunk = new VLTExportChunk(dataChunk.ExportEntries);
            chunkWriter.WriteChunk(exportChunk);
            var binWriter = new BinaryWriter(BinStream);

            foreach (var pointerObject in _exports.OfType<IPointerObject>())
                pointerObject.WritePointerData(_vault, binWriter);

            // after writing exports, we can build pointers
            BuildPointers();

            var pointersChunk = new VLTPointersChunk();
            chunkWriter.WriteChunk(pointersChunk);
            var endChunk = new EndChunk();
            chunkWriter.WriteChunk(endChunk);
        }

        private IEnumerable<string> CollectStrings(VLTCollection collection)
        {
            return collection.DataRow.Values.OfType<IReferencesStrings>().SelectMany(r => r.GetStrings());
        }
    }
}