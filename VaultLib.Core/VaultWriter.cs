using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core.Chunks;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;
using VaultLib.Core.Writer;

namespace VaultLib.Core;

/// <summary>
/// Generates BIN and VLT data streams for a <see cref="Vault{TKey}"/> instance.
/// </summary>
public class VaultWriter<TKey> where TKey : struct, IKey<TKey>
{
    private readonly VaultWriteContext<TKey> _writeContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="VaultWriter{TKey}"/> class.
    /// </summary>
    /// <param name="vault">The <see cref="Vault{TKey}"/> instance to generate data for.</param>
    /// <param name="options">The options for the saving process.</param>
    public VaultWriter(Vault<TKey> vault, VaultWriteOptions options)
    {
        Vault = vault;
        Options = options;

        _writeContext = new VaultWriteContext<TKey>(vault, options);

        ExportManager = new VaultExportManager<TKey>(_writeContext);
        ExportManager.BuildVaultExports();

#if DEBUG
        var seenCollections = new HashSet<VltCollection<TKey>>();
        foreach (var collectionExport in ExportManager.GetExports().OfType<BaseCollectionLoad<TKey>>())
        {
            var collection = collectionExport.Collection;
            if (collection.Parent is { } parentCollection)
            {
                if (ReferenceEquals(parentCollection.Vault, vault) && !seenCollections.Contains(parentCollection))
                {
                    var collectionPath = $"{collection.Class.Key}/{collection.Key}";
                    var parentCollectionPath = $"{parentCollection.Class.Key}/{parentCollection.Key}";
                    
                    throw new Exception(
                        $"Collection {collectionPath} should not be written before parent {parentCollectionPath}!!!");
                }
            }

            seenCollections.Add(collection);
        }
#endif
    }

    /// <summary>
    /// Gets the vault to generate data for.
    /// </summary>
    public Vault<TKey> Vault { get; }

    /// <summary>
    /// Gets the options for the saving process. 
    /// </summary>
    public VaultWriteOptions Options { get; }

    /// <summary>
    /// Gets the export manager.
    /// </summary>
    public VaultExportManager<TKey> ExportManager { get; }

    /// <summary>
    /// Builds BIN and VLT streams for the vault and returns them.
    /// </summary>
    /// <returns>The generated streams.</returns>
    public VaultStreamInfo BuildVault()
    {
        ExportManager.PrepareExports();

        var binStream = BuildBinStream();
        var vltStream = BuildVltStream(binStream);

        binStream.Position = vltStream.Position = 0;

        Debug.WriteLine("[OUT] vault {0}: bin size 0x{1:X} vlt size 0x{2:X}", Vault.Name, binStream.Length,
            vltStream.Length);

        return new VaultStreamInfo(binStream, vltStream);
    }

    #region Internal Implementation

    private Stream BuildBinStream()
    {
        MemoryStream ms = new MemoryStream(8192);
        BinaryWriter bw = new BinaryWriter(ms);

        ChunkWriter<TKey> cw = new ChunkWriter<TKey>(bw, _writeContext);
        var stringsSet = new HashSet<string>();

        var strings = _writeContext.Collections.SelectMany(CollectStrings).ToList();
        stringsSet.UnionWith(strings);
        var stringsChunk = new BinStringsChunk<TKey> { Strings = new List<string>(stringsSet) };

        cw.WriteChunk(stringsChunk);

        if (_writeContext.Options.Quirks.EnableBinEndChunk)
        {
            cw.WriteChunk(new EndChunk<TKey>());
        }

        return ms;
    }

    private Stream BuildVltStream(Stream binStream)
    {
        MemoryStream ms = new MemoryStream(8192);
        BinaryWriter bw = new BinaryWriter(ms);
        ChunkWriter<TKey> cw = new ChunkWriter<TKey>(bw, _writeContext);

        var versionChunk = new VltVersionChunk<TKey>();
        cw.WriteChunk(versionChunk);

        var startChunk = new VltStartChunk<TKey>();
        var dependencyChunk = new VltDependencyChunk<TKey>(new List<string>
        {
            $"{Vault.Name}.vlt",
            $"{Vault.Name}.bin"
        });

        if (Options.Quirks.StartChunkBeforeDepChunk)
        {
            cw.WriteChunk(startChunk);
            cw.WriteChunk(dependencyChunk);
        }
        else
        {
            cw.WriteChunk(dependencyChunk);
            cw.WriteChunk(startChunk);
        }

        var dataChunk = new VltDataChunk<TKey>(ExportManager.GetExports());
        cw.WriteChunk(dataChunk);

        var exportChunk = new VltExportChunk<TKey>(dataChunk.ExportEntries);
        cw.WriteChunk(exportChunk);
        var binWriter = new BinaryWriter(binStream);

        foreach (var pointerObject in ExportManager.GetExports().OfType<IPointerObject<TKey>>())
            pointerObject.WritePointerData(_writeContext, binWriter);

        // after writing exports, we can build pointers
        BuildPointers();

        var pointersChunk = new VltPointersChunk<TKey>();
        cw.WriteChunk(pointersChunk);

        if (_writeContext.Options.Quirks.EnableVltEndChunk)
        {
            var endChunk = new EndChunk<TKey>();
            cw.WriteChunk(endChunk);
        }

        return ms;
    }

    private void BuildPointers()
    {
        foreach (var pointerObject in ExportManager.GetExports().OfType<IPointerObject<TKey>>())
            pointerObject.AddPointers(_writeContext);
    }

    private static IEnumerable<string> CollectStrings(VltCollection<TKey> collection)
    {
        foreach (var value in collection.GetData().Values)
        {
            switch (value)
            {
                case string stringValue:
                    yield return stringValue;
                    break;
                case IReferencesStrings referencesStrings:
                {
                    foreach (var s in referencesStrings.GetStrings())
                    {
                        yield return s;
                    }

                    break;
                }
            }
        }
    }

    #endregion
}