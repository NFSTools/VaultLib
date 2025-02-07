// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:32 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;

namespace VaultLib.Core.Chunks;

public class VltExportChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    private readonly List<IExportEntry<TKey>> _exports;

    public VltExportChunk()
    {
    }

    public VltExportChunk(List<IExportEntry<TKey>> exports)
    {
        _exports = exports;
    }

    public override uint Id => 0x4578704E;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        var numExports = TKey.Size == 8 ? br.ReadUInt64() : br.ReadUInt32();
        for (ulong i = 0; i < numExports; i++)
        {
            var exportEntry = context.Database.ExportFactory.BuildExportEntry();

            exportEntry.Read(context, br);

            var export = context.Database.ExportFactory.CreateExport(exportEntry.Type);

            Debug.Assert(export != null);

            export.Offset = exportEntry.Offset;
            export.Size = exportEntry.Size;
            context.Exports.Add(export);
        }
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        //bw.Write(_exports.Count);
        if (TKey.Size == 8)
            bw.Write((ulong)_exports.Count);
        else
            bw.Write(_exports.Count);

        foreach (var exportEntry in _exports) exportEntry.Write(context, bw);

        bw.AlignWriter(0x10);
    }
}