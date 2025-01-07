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
    public static class ModuleDef
    {
        public static void Load(TypeRegistry typeRegistry)
        {
            typeRegistry.Register<StringKey>("Attrib::StringKey");
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_UNDERCOVER);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_UNDERCOVER);

            SpeedFramework.Register(typeRegistry);
            typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
        }
    }
}