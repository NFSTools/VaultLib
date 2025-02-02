using System;
using System.Collections.Generic;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Structures;

namespace VaultLib.Core.Exports;

public class ExportFactory<TKey> where TKey : struct, IKey<TKey>
{
    private readonly Func<BaseDatabaseLoad<TKey>> _databaseLoadFactory;
    private readonly Func<BaseClassLoad<TKey>> _classLoadFactory;
    private readonly Func<BaseCollectionLoad<TKey>> _collectionLoadFactory;
    private readonly Func<IExportEntry<TKey>> _exportEntryFactory;
    private readonly Func<IPtrRef<TKey>> _ptrRefFactory;

    private Dictionary<TKey, Func<BaseExport<TKey>>> ExportBuilders { get; } = new();

    private static readonly TKey ClassLoadDataKey = TKey.FromString("Attrib::ClassLoadData");
    private static readonly TKey CollectionLoadDataKey = TKey.FromString("Attrib::CollectionLoadData");
    private static readonly TKey DatabaseLoadDataKey = TKey.FromString("Attrib::DatabaseLoadData");

    public ExportFactory(Func<BaseDatabaseLoad<TKey>> databaseLoadFactory,
        Func<BaseClassLoad<TKey>> classLoadFactory, Func<BaseCollectionLoad<TKey>> collectionLoadFactory,
        Func<IExportEntry<TKey>> exportEntryFactory, Func<IPtrRef<TKey>>? ptrRefFactory = null)
    {
        _databaseLoadFactory = databaseLoadFactory;
        _classLoadFactory = classLoadFactory;
        _collectionLoadFactory = collectionLoadFactory;
        _exportEntryFactory = exportEntryFactory;
        _ptrRefFactory = ptrRefFactory ?? (() => new AttribPtrRef<TKey>());

        ExportBuilders.Add(ClassLoadDataKey, classLoadFactory);
        ExportBuilders.Add(CollectionLoadDataKey, collectionLoadFactory);
        ExportBuilders.Add(DatabaseLoadDataKey, databaseLoadFactory);
    }

    public void RegisterExportType<TExport>(TKey exportType) where TExport : BaseExport<TKey>, new()
    {
        ExportBuilders.Add(exportType, () => new TExport());
    }

    public BaseExport<TKey> CreateExport(TKey exportType)
    {
        if (!ExportBuilders.TryGetValue(exportType, out var exportBuilder))
        {
            throw new KeyNotFoundException($"No factory found for export type: {exportType}");
        }

        return exportBuilder();
    }

    public BaseCollectionLoad<TKey> BuildCollectionLoad(VltCollection<TKey> collection)
    {
        var collectionLoad = _collectionLoadFactory();
        collectionLoad.Collection = collection;

        return collectionLoad;
    }

    public BaseClassLoad<TKey> BuildClassLoad(VltClass<TKey> vltClass)
    {
        var classLoad = _classLoadFactory();

        classLoad.Class = vltClass;
        return classLoad;
    }

    public BaseDatabaseLoad<TKey> BuildDatabaseLoad()
    {
        return _databaseLoadFactory();
    }

    public IPtrRef<TKey> CreatePtrRef()
    {
        return _ptrRefFactory();
    }

    public IExportEntry<TKey> BuildExportEntry()
    {
        return _exportEntryFactory();
    }
}