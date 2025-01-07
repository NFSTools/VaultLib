using System.Reflection;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.Carbon
{
    public static class ModuleDef
    {
        public static void Load(TypeRegistry typeRegistry)
        {
            typeRegistry.Register<StringKey>("Attrib::StringKey");
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_CARBON);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_CARBON);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_CARBON);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_CARBON);

            SpeedFramework.Register(typeRegistry);
            typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
        }
    }
}
