using System.ComponentModel.Composition;
using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;

namespace VaultLib.Support.MostWanted
{
    [DataModuleInfo("VLT Support - Most Wanted", "heyitsleo", games: GameIdHelper.ID_MW)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()), GameIdHelper.ID_MW);
            GameIdHelper.AddFeature(GameIdHelper.ID_MW, "VLT");
        }
    }
}
