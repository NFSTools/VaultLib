using System.ComponentModel.Composition;
using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.LegacyBase;
using VaultLib.LegacyBase.Exports;
using VaultLib.LegacyBase.Structures;

namespace VaultLib.Support.MostWanted
{
    [DataModuleInfo("VLT Support - Most Wanted", "heyitsleo", games: GameIdHelper.ID_MW)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey64>("Attrib::StringKey", GameIdHelper.ID_MW);
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()), GameIdHelper.ID_MW);
            ExportFactory.SetClassLoadCreator<LegacyClassLoad>(GameIdHelper.ID_MW);
            ExportFactory.SetCollectionLoadCreator<LegacyCollectionLoad>(GameIdHelper.ID_MW);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_MW);
            ExportFactory.SetExportEntryCreator<LegacyExportEntry>(GameIdHelper.ID_MW);
            GameIdHelper.AddFeature(GameIdHelper.ID_MW, "VLT");
        }
    }
}
