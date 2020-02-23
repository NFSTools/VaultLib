// This file is part of VaultLib.Support.Undercover by heyitsleo.
// 
// Created: 10/31/2019 @ 10:01 PM.

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

namespace VaultLib.Support.Undercover
{
    [DataModuleInfo("VLT Support - Undercover", "heyitsleo", games: GameIdHelper.ID_UNDERCOVER)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_UNDERCOVER);

            SpeedFramework.Register(GameIdHelper.ID_UNDERCOVER);
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)), GameIdHelper.ID_UNDERCOVER);
        }
    }
}