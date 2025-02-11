using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;
using VaultLib.LegacyBase;
using VaultLib.LegacyBase.Exports;
using VaultLib.LegacyBase.Structures;

namespace VaultLib.Support.MostWanted;

public class ModuleDef32 : BaseGameModule<Key32>
{
    public override void RegisterTypes(TypeRegistryBuilder<Key32> typeRegistry)
    {
        typeRegistry.Register<StringKey64>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(typeof(ModuleDef32).Assembly);
    }

    public override ExportFactory<Key32> CreateExportFactory()
    {
        return new ExportFactory<Key32>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad32(),
            () => new ExportEntry32());
    }
}