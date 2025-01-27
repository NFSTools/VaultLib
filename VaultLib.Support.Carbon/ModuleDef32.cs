using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.Carbon;

public class ModuleDef32 : BaseGameModule<uint>
{
    public override void RegisterTypes(TypeRegistry<uint> typeRegistry)
    {
        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef32)));
    }

    public override ExportFactory<uint> CreateExportFactory()
    {
        return new ExportFactory<uint>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad(),
            () => new ExportEntry32());
    }
}