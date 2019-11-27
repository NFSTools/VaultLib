// This file is part of VaultLib.Support.World by heyitsleo.
// 
// Created: 11/02/2019 @ 1:32 PM.

using System.ComponentModel.Composition;
using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.ModuleHelper;
using VaultLib.Core.Types.Attrib;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.World
{
    [DataModuleInfo("VLT Support - World", "heyitsleo", games: GameIdHelper.ID_WORLD)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : BaseVLTModule
    {
        public override void Load()
        {
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)), GameIdHelper.ID_WORLD);
            ExportFactory.SetClassLoadCreator<ModernClassLoad>(GameIdHelper.ID_WORLD);
            ExportFactory.SetCollectionLoadCreator<ModernCollectionLoad>(GameIdHelper.ID_WORLD);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_WORLD);
            ExportFactory.SetExportEntryCreator<ModernExportEntry>(GameIdHelper.ID_WORLD);

            GameIdHelper.AddFeature(GameIdHelper.ID_WORLD, VLTFeatureKey);
        }
    }
}