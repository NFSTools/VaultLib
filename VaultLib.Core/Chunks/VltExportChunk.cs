// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:32 PM.

using CoreLibraries.IO;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;

namespace VaultLib.Core.Chunks;

public class VltExportChunk : ChunkBase
{
    private readonly List<IExportEntry> _exports;

    public VltExportChunk()
    {
    }

    public VltExportChunk(List<IExportEntry> exports)
    {
        _exports = exports;
    }

    public override uint Id => 0x4578704E;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext context, BinaryReader br)
    {
        var numExports = context.Database.Options.Type == DatabaseType.X64Database ? br.ReadUInt64() : br.ReadUInt32();
        for (ulong i = 0; i < numExports; i++)
        {
            var exportEntry = context.Database.ExportFactory.BuildExportEntry();

            exportEntry.Read(context, br);

            var export = CreateExport(context, exportEntry.Type);

            if (export == null) continue;

            export.Offset = exportEntry.Offset;
            export.Size = exportEntry.Size;
            context.Vault.Exports.Add(export);
        }
    }

    public override void Write(VaultWriteContext context, BinaryWriter bw)
    {
        //bw.Write(_exports.Count);
        if (context.Database.Options.Type == DatabaseType.X64Database)
            bw.Write((ulong)_exports.Count);
        else
            bw.Write(_exports.Count);

        foreach (var exportEntry in _exports) exportEntry.Write(context, bw);

        bw.AlignWriter(0x10);
    }

    private BaseExport CreateExport(VaultReadContext context, ulong type)
    {
        switch (type)
        {
            case 0x5E970CBC: // Attrib::ClassLoadData
            case 0x2A7895AC4A876152: // Attrib::ClassLoadData
                return context.Database.ExportFactory.BuildClassLoad(null);
            case 0xCBBC628F: // Attrib::DatabaseLoadData
            case 0xB38846845E9C175: // Attrib::DatabaseLoadData
                return context.Database.ExportFactory.BuildDatabaseLoad();
            case 0x8E112EB7:
            case 0xAD303B8F42B3307E:
                return context.Database.ExportFactory.BuildCollectionLoad(null);
            default:
                return null;
        }
    }
}