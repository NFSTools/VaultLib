// This file is part of VaultLib.Support.ProStreet by heyitsleo.
// 
// Created: 10/31/2019 @ 9:54 PM.

using System.ComponentModel.Composition;
using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;
using VaultLib.ModernBase;

namespace VaultLib.Support.ProStreet
{
    [DataModuleInfo("VLT Support - ProStreet", "heyitsleo", games: GameIdHelper.ID_PROSTREET)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", GameIdHelper.ID_PROSTREET);
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)), GameIdHelper.ID_PROSTREET);
            GameIdHelper.AddFeature(GameIdHelper.ID_PROSTREET, "VLT");
        }
    }
}