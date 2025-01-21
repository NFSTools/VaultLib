// This file is part of VaultLib.Support.World by heyitsleo.
// 
// Created: 11/02/2019 @ 1:32 PM.

using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.World;

public class ModuleDef : BaseGameModule
{
    public override void RegisterTypes(TypeRegistry typeRegistry)
    {
        typeRegistry.Register<StringKey>("Attrib::StringKey");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
    }

    public override ExportFactory CreateExportFactory()
    {
        return new ExportFactory(() => new DatabaseLoad(), () => new ClassLoad(), () => new CollectionLoad(),
            () => new ExportEntry());
    }
}