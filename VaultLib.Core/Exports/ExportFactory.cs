using System;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Structures;

namespace VaultLib.Core.Exports;

public class ExportFactory<TKey> where TKey : IKey<TKey>
{
    // private static readonly Dictionary<string, Func<IExportEntry>> ExportEntryCreatorDictionary =
    //     new Dictionary<string, Func<IExportEntry>>();
    //
    // private static readonly Dictionary<string, Func<IPtrRef>> PtrCreatorDictionary =
    //     new Dictionary<string, Func<IPtrRef>>();
    //
    // private static readonly Dictionary<string, Func<BaseCollectionLoad>> CollectionLoadBuilderDictionary =
    //     new Dictionary<string, Func<BaseCollectionLoad>>();
    //
    // private static readonly Dictionary<string, Func<BaseClassLoad>> ClassLoadBuilderDictionary =
    //     new Dictionary<string, Func<BaseClassLoad>>();
    //
    // private static readonly Dictionary<string, Func<BaseDatabaseLoad>> DatabaseLoadBuilderDictionary =
    //     new Dictionary<string, Func<BaseDatabaseLoad>>();

    // public static void SetPointerCreator<T>(string game) where T : IPtrRef, new()
    // {
    //     PtrCreatorDictionary.Add(game, () => new T());
    // }
    //
    // public static void SetExportEntryCreator<T>(string game) where T : IExportEntry, new()
    // {
    //     ExportEntryCreatorDictionary.Add(game, () => new T());
    // }
    //
    // public static void SetCollectionLoadCreator<T>(string game) where T : BaseCollectionLoad, new()
    // {
    //     CollectionLoadBuilderDictionary.Add(game, () => new T());
    // }
    //
    // public static void SetClassLoadCreator<T>(string game) where T : BaseClassLoad, new()
    // {
    //     ClassLoadBuilderDictionary.Add(game, () => new T());
    // }
    //
    // public static void SetDatabaseLoadCreator<T>(string game) where T : BaseDatabaseLoad, new()
    // {
    //     DatabaseLoadBuilderDictionary.Add(game, () => new T());
    // }

    private readonly Func<BaseDatabaseLoad<TKey>> _databaseLoadFactory;
    private readonly Func<BaseClassLoad<TKey>> _classLoadFactory;
    private readonly Func<BaseCollectionLoad<TKey>> _collectionLoadFactory;
    private readonly Func<IExportEntry<TKey>> _exportEntryFactory;
    private readonly Func<IPtrRef<TKey>> _ptrRefFactory;

    public ExportFactory(Func<BaseDatabaseLoad<TKey>> databaseLoadFactory,
        Func<BaseClassLoad<TKey>> classLoadFactory, Func<BaseCollectionLoad<TKey>> collectionLoadFactory,
        Func<IExportEntry<TKey>> exportEntryFactory, Func<IPtrRef<TKey>> ptrRefFactory = null)
    {
        _databaseLoadFactory = databaseLoadFactory;
        _classLoadFactory = classLoadFactory;
        _collectionLoadFactory = collectionLoadFactory;
        _exportEntryFactory = exportEntryFactory;
        _ptrRefFactory = ptrRefFactory ?? (() => new AttribPtrRef<TKey>());
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