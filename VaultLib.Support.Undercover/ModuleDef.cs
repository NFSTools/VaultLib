// This file is part of VaultLib.Support.Undercover by heyitsleo.
// 
// Created: 10/31/2019 @ 10:01 PM.

using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.Undercover;

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