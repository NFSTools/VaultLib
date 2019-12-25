// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 3:30 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;

namespace VaultLib.Core.Chunks
{
    public class VltDataChunk : ChunkBase
    {
        private readonly IList<BaseExport> _exports;

        public VltDataChunk(IList<BaseExport> exports)
        {
            _exports = exports;
            ExportEntries = new List<IExportEntry>();
        }

        public List<IExportEntry> ExportEntries { get; }

        public override uint Id => 0x4461744E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            foreach (var t in _exports)
            {
                var offset = bw.BaseStream.Position;

                t.Write(vault, bw);

                var endOffset = bw.BaseStream.Position;

                var exportEntry = ExportFactory.BuildExportEntry(vault);
                exportEntry.ID = t.GetExportID();
                exportEntry.Offset = (uint)offset;
                exportEntry.Type = vault.SaveContext.StringHash(t.GetTypeId());
                exportEntry.Size = (uint)(endOffset - offset);

                ExportEntries.Add(exportEntry);

                bw.AlignWriter(8);
            }
        }
    }
}