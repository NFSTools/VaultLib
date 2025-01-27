using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.Carbon;

public class ModuleDef32 : BaseGameModule<Key32>
{
    public override void RegisterTypes(TypeRegistry<Key32> typeRegistry)
    {
        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef32)));
    }

    public override ExportFactory<Key32> CreateExportFactory()
    {
        return new ExportFactory<Key32>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad(),
            () => new ExportEntry32());
    }
}