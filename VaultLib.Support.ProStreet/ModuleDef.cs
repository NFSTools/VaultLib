// This file is part of VaultLib.Support.ProStreet by heyitsleo.
// 
// Created: 10/31/2019 @ 9:54 PM.

using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.ProStreet;

public class ModuleDef : BaseGameModule<uint>
{
    public override void RegisterTypes(TypeRegistry<uint> typeRegistry)
    {
        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
    }

    public override ExportFactory<uint> CreateExportFactory()
    {
        return new ExportFactory<uint>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad(),
            () => new ExportEntry32());
    }
}