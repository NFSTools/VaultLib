// This file is part of VaultLib.Support.World by heyitsleo.
// 
// Created: 11/02/2019 @ 1:32 PM.

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

namespace VaultLib.Support.World
{
    public static class ModuleDef
    {
        public static void Load(TypeRegistry typeRegistry)
        {
            typeRegistry.Register<StringKey>("Attrib::StringKey");
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_WORLD);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_WORLD);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_WORLD);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_WORLD);

            SpeedFramework.Register(typeRegistry);
            typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
        }
    }
}