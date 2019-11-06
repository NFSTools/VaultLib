// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:32 PM.

using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;

namespace VaultLib.Core.Chunks
{
    public class VLTExportChunk : ChunkBase
    {
        private List<IExportEntry> _exports;

        public VLTExportChunk() { }

        public VLTExportChunk(List<IExportEntry> exports)
        {
            _exports = exports;
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            var numExports = br.ReadUInt32();

            for (int i = 0; i < numExports; i++)
            {
                var exportEntry = ExportFactory.BuildExportEntry(vault);

                exportEntry.Read(vault, br);

                BaseExport export = this.CreateExport(vault, exportEntry.Type);

                if (export != null)
                {
                    export.Offset = exportEntry.Offset;
                    vault.Exports.Add(export);
                }
                //Debug.WriteLine("Export ID {0:X8} Type {1:X8} Offset {2:X} Size {3}", exportEntry.ID, exportEntry.Type, exportEntry.Offset, exportEntry.Size);
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(_exports.Count);

            foreach (var exportEntry in _exports)
            {
                exportEntry.Write(vault, bw);
            }

            bw.AlignWriter(0x10);
        }

        private BaseExport CreateExport(Vault vault, ulong type)
        {
            switch (type)
            {
                case 0x5E970CBC: // Attrib::ClassLoadData
                case 0x2A7895AC4A876152: // Attrib::ClassLoadData
                    return ExportFactory.BuildClassLoad(vault, null);
                case 0xCBBC628F: // Attrib::DatabaseLoadData
                case 0xB38846845E9C175: // Attrib::DatabaseLoadData
                    return ExportFactory.BuildDatabaseLoad(vault);
                case 0x8E112EB7:
                case 0xAD303B8F42B3307E:
                    return ExportFactory.BuildCollectionLoad(vault, null);
                default:
                    return null;
            }
        }

        public override uint ID => 0x4578704E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }
    }
}