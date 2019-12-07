using System;
using System.Collections.Generic;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Structures;

namespace VaultLib.Core.Exports
{
    public static class ExportFactory
    {
        private static readonly Dictionary<string, Func<IExportEntry>> ExportEntryCreatorDictionary =
            new Dictionary<string, Func<IExportEntry>>();

        private static readonly Dictionary<string, Func<IPtrRef>> PtrCreatorDictionary =
            new Dictionary<string, Func<IPtrRef>>();

        private static readonly Dictionary<string, Func<BaseCollectionLoad>> CollectionLoadBuilderDictionary =
            new Dictionary<string, Func<BaseCollectionLoad>>();

        private static readonly Dictionary<string, Func<BaseClassLoad>> ClassLoadBuilderDictionary =
            new Dictionary<string, Func<BaseClassLoad>>();

        private static readonly Dictionary<string, Func<BaseDatabaseLoad>> DatabaseLoadBuilderDictionary =
            new Dictionary<string, Func<BaseDatabaseLoad>>();

        public static void SetPointerCreator<T>(string game) where T : IPtrRef, new()
        {
            PtrCreatorDictionary.Add(game, () => new T());
        }

        public static void SetExportEntryCreator<T>(string game) where T : IExportEntry, new()
        {
            ExportEntryCreatorDictionary.Add(game, () => new T());
        }

        public static void SetCollectionLoadCreator<T>(string game) where T : BaseCollectionLoad, new()
        {
            CollectionLoadBuilderDictionary.Add(game, () => new T());
        }

        public static void SetClassLoadCreator<T>(string game) where T : BaseClassLoad, new()
        {
            ClassLoadBuilderDictionary.Add(game, () => new T());
        }

        public static void SetDatabaseLoadCreator<T>(string game) where T : BaseDatabaseLoad, new()
        {
            DatabaseLoadBuilderDictionary.Add(game, () => new T());
        }

        public static IExportEntry BuildExportEntry(Vault vault)
        {
            return ExportEntryCreatorDictionary[vault.Database.Options.GameId]();
        }

        public static BaseCollectionLoad BuildCollectionLoad(Vault vault, VLTCollection collection)
        {
            var collectionLoad = CollectionLoadBuilderDictionary[vault.Database.Options.GameId]();

            collectionLoad.Collection = collection;

            return collectionLoad;
        }

        public static BaseClassLoad BuildClassLoad(Vault vault, VLTClass vltClass)
        {
            var classLoad = ClassLoadBuilderDictionary[vault.Database.Options.GameId]();

            classLoad.Class = vltClass;
            return classLoad;
        }

        public static BaseDatabaseLoad BuildDatabaseLoad(Vault vault)
        {
            return DatabaseLoadBuilderDictionary[vault.Database.Options.GameId]();
        }

        public static IPtrRef CreatePtrRef(Vault vault)
        {
            if (PtrCreatorDictionary.TryGetValue(vault.Database.Options.GameId, out var creator))
                return creator();
            return new AttribPtrRef();
        }
    }
}