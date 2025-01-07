using System.Reflection;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.LegacyBase;
using VaultLib.LegacyBase.Exports;
using VaultLib.LegacyBase.Structures;

namespace VaultLib.Support.MostWanted
{
    public static class ModuleDef
    {
        public static void Load(TypeRegistry typeRegistry)
        {
            typeRegistry.Register<StringKey64>("Attrib::StringKey");
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_MW);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_MW);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_MW);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_MW);

            SpeedFramework.Register(typeRegistry);
            typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
        }
    }
}
