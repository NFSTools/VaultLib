// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 3:30 PM.

using System;
using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;

namespace VaultLib.Core.Chunks;

public class VltDataChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    private readonly IList<BaseExport<TKey>> _exports;

    public VltDataChunk(IList<BaseExport<TKey>> exports)
    {
        _exports = exports;
        ExportEntries = new List<IExportEntry<TKey>>();
    }

    public List<IExportEntry<TKey>> ExportEntries { get; }

    public override uint Id => 0x4461744E;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        throw new NotImplementedException();
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        foreach (var t in _exports)
        {
            var offset = bw.BaseStream.Position;

            t.Write(context, bw);

            var endOffset = bw.BaseStream.Position;

            var exportEntry = context.Database.ExportFactory.BuildExportEntry();
            exportEntry.Id = t.GetExportId();
            exportEntry.Offset = (uint)offset;
            exportEntry.Type = context.StringToKey(t.GetTypeId());
            exportEntry.Size = (uint)(endOffset - offset);

            ExportEntries.Add(exportEntry);

            bw.AlignWriter(8);
        }

        bw.AlignWriter(0x10);
    }
}