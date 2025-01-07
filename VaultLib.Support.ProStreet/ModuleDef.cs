// This file is part of VaultLib.Support.ProStreet by heyitsleo.
// 
// Created: 10/31/2019 @ 9:54 PM.

using System.Reflection;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.ProStreet
{
    public class ModuleDef
    {
        public void Load(TypeRegistry typeRegistry)
        {
            typeRegistry.Register<StringKey>("Attrib::StringKey");
            ExportFactory.SetClassLoadCreator<ClassLoad>(GameIdHelper.ID_PROSTREET);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(GameIdHelper.ID_PROSTREET);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(GameIdHelper.ID_PROSTREET);
            ExportFactory.SetExportEntryCreator<ExportEntry>(GameIdHelper.ID_PROSTREET);

            SpeedFramework.Register(typeRegistry);
            typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
        }
    }
}