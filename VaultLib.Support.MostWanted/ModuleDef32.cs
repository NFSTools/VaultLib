using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.LegacyBase;
using VaultLib.LegacyBase.Exports;
using VaultLib.LegacyBase.Structures;

namespace VaultLib.Support.MostWanted;

public class ModuleDef32 : BaseGameModule<uint>
{
    public override void RegisterTypes(TypeRegistry<uint> typeRegistry)
    {
        typeRegistry.Register<StringKey64>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef32)));
    }

    public override ExportFactory<uint> CreateExportFactory()
    {
        return new ExportFactory<uint>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad32(),
            () => new ExportEntry32());
    }
}