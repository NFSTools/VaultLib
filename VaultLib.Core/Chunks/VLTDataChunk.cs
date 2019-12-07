// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 3:30 PM.

using System;
using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;

namespace VaultLib.Core.Chunks
{
    public class VLTDataChunk : ChunkBase
    {
        private readonly List<BaseExport> _exports;

        public VLTDataChunk(List<BaseExport> exports)
        {
            _exports = exports;
            ExportEntries = new List<IExportEntry>();
        }

        public List<IExportEntry> ExportEntries { get; }

        public override uint ID => 0x4461744E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            //Debug.WriteLine("writing exports", vault.Name);
            for (var i = 0; i < _exports.Count; i++)
            {
                //Debug.WriteLine("Writing export {0}/{1}", i + 1, _exports.Count);

                var offset = bw.BaseStream.Position;

                _exports[i].Write(vault, bw);

                var endOffset = bw.BaseStream.Position;

                var exportEntry = ExportFactory.BuildExportEntry(vault);
                exportEntry.ID = _exports[i].GetExportID();
                exportEntry.Offset = (uint) offset;
                exportEntry.Type = _exports[i].GetTypeId();
                exportEntry.Size = (uint) (endOffset - offset);

                ExportEntries.Add(exportEntry);

                //Debug.WriteLine("wrote export {0} ({3:X}) - offset {1:X} size {2}", _exports[i], exportEntry.Offset, exportEntry.Size, exportEntry.ID);

                bw.AlignWriter(0x8);
            }
        }
    }
}