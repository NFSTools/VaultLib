// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:32 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;

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
        var numExports = context.Database.Options.Type == DatabaseType.X64Database ? br.ReadUInt64() : br.ReadUInt32();
        for (ulong i = 0; i < numExports; i++)
        {
            var exportEntry = context.Database.ExportFactory.BuildExportEntry();

            exportEntry.Read(context, br);

            var export = CreateExport(context, exportEntry.Type);

            Debug.Assert(export != null);

            export.Offset = exportEntry.Offset;
            export.Size = exportEntry.Size;
            context.Vault.Exports.Add(export);
        }
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        //bw.Write(_exports.Count);
        if (context.Database.Options.Type == DatabaseType.X64Database)
            bw.Write((ulong)_exports.Count);
        else
            bw.Write(_exports.Count);

        foreach (var exportEntry in _exports) exportEntry.Write(context, bw);

        bw.AlignWriter(0x10);
    }

    private BaseExport<TKey> CreateExport(VaultReadContext<TKey> context, TKey type)
    {
        // TODO: these shouldn't be hardcoded

        switch (type)
        {
            case Key64 k64:
                switch (k64.Hash)
                {
                    case 0x2A7895AC4A876152u: // Attrib::ClassLoadData
                        return context.Database.ExportFactory.BuildClassLoad(null);
                    case 0xB38846845E9C175u: // Attrib::DatabaseLoadData
                        return context.Database.ExportFactory.BuildDatabaseLoad();
                    case 0xAD303B8F42B3307Eu:
                        return context.Database.ExportFactory.BuildCollectionLoad(null);
                }

                break;
            case Key32 k32:
                switch (k32.Hash)
                {
                    case 0x5E970CBCu: // Attrib::ClassLoadData
                        return context.Database.ExportFactory.BuildClassLoad(null);
                    case 0xCBBC628Fu: // Attrib::DatabaseLoadData
                        return context.Database.ExportFactory.BuildDatabaseLoad();
                    case 0x8E112EB7u:
                        return context.Database.ExportFactory.BuildCollectionLoad(null);
                }

                break;
        }

        return null;
    }
}