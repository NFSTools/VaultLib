using System.ComponentModel.Composition;
using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;

namespace VaultLib.Support.Carbon
{
    [DataModuleInfo("VLT Support - Carbon", "heyitsleo", games: GameIdHelper.ID_CARBON)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()), GameIdHelper.ID_CARBON);
            GameIdHelper.AddFeature(GameIdHelper.ID_CARBON, "VLT");
        }
    }
}
