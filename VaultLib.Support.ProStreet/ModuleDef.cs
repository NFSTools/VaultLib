// This file is part of VaultLib.Support.ProStreet by heyitsleo.
// 
// Created: 10/31/2019 @ 9:54 PM.

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

namespace VaultLib.Support.ProStreet
{
    [DataModuleInfo("VLT Support - ProStreet", "heyitsleo", games: GameIdHelper.ID_PROSTREET)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", GameIdHelper.ID_PROSTREET);
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_PROSTREET);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_PROSTREET);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_PROSTREET);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_PROSTREET);

            SpeedFramework.Register(GameIdHelper.ID_PROSTREET);
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)), GameIdHelper.ID_PROSTREET);
        }
    }
}