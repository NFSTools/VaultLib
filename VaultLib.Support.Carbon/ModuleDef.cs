using System.ComponentModel.Composition;
using System.Reflection;
using CoreLibraries.GameUtilities;
using CoreLibraries.ModuleSystem;
using VaultLib.Core;
using VaultLib.ModernBase;

namespace VaultLib.Support.Carbon
{
    [DataModuleInfo("VLT Support - Carbon", "heyitsleo", games: GameIdHelper.ID_CARBON)]
    [Export(typeof(IDataModule))]
    public class ModuleDef : IDataModule
    {
        public void Load()
        {
            TypeRegistry.Register<StringKey>("Attrib::StringKey", GameIdHelper.ID_CARBON);
            TypeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(GetType()), GameIdHelper.ID_CARBON);
            GameIdHelper.AddFeature(GameIdHelper.ID_CARBON, "VLT");
        }
    }
}
