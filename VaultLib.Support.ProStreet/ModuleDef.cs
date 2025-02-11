// This file is part of VaultLib.Support.ProStreet by heyitsleo.
// 
// Created: 10/31/2019 @ 9:54 PM.

using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.ProStreet;

public class ModuleDef : BaseGameModule<Key32>
{
    public override void RegisterTypes(TypeRegistryBuilder<Key32> typeRegistry)
    {
        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(typeof(ModuleDef).Assembly);
    }

    public override ExportFactory<Key32> CreateExportFactory()
    {
        return new ExportFactory<Key32>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad(),
            () => new ExportEntry32());
    }
}