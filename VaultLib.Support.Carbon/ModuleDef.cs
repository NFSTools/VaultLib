using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using System.ComponentModel.Composition;
using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.Carbon
{
    [DataModuleInfo("VLT Support - Carbon", "heyitsleo", games: GameIdHelper.ID_CARBON)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", GameIdHelper.ID_CARBON);
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_CARBON);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_CARBON);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_CARBON);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_CARBON);

            SpeedFramework.Register(GameIdHelper.ID_CARBON);
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()), GameIdHelper.ID_CARBON);
        }
    }
}
