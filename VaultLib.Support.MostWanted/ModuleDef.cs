using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.LegacyBase;
using VaultLib.LegacyBase.Exports;
using VaultLib.LegacyBase.Structures;

namespace VaultLib.Support.MostWanted
{
    public class ModuleDef : BaseGameModule
    {
        public override void RegisterTypes(TypeRegistry typeRegistry)
        {
            typeRegistry.Register<StringKey64>("Attrib::StringKey");
            SpeedFramework.Register(typeRegistry);
            typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
        }

        public override ExportFactory CreateExportFactory()
        {
            return new ExportFactory(() => new DatabaseLoad(), () => new ClassLoad(), () => new CollectionLoad(),
                () => new ExportEntry());
        }
    }
}