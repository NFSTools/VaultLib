using CoreLibraries.ModuleSystem;
using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.TheRun
{
    [DataModuleInfo("VLT Support - The Run", "heyitsleo", games: "THE_RUN")]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", "THE_RUN");
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)), "THE_RUN");
            ExportFactory.SetClassLoadCreator<ClassLoad>("THE_RUN");
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>("THE_RUN");
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>("THE_RUN");
            ExportFactory.SetExportEntryCreator<ExportEntry>("THE_RUN");
        }
    }
}