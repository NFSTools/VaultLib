// This file is part of VaultLib.Support.Undercover by heyitsleo.
// 
// Created: 10/31/2019 @ 10:01 PM.

using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Types.Attrib.Query;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace VaultLib.Support.Undercover;

public class ModuleDef : BaseGameModule<Key32>
{
    public override void RegisterTypes(TypeRegistry<Key32> typeRegistry)
    {
        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        typeRegistry.Register<Static_Inorder_N_to_1>("Attrib::Query::Static_Inorder_N_to_1<Attrib::Query::Typespace<Attrib::Key,Attrib::Key,EA::Reflection::UInt32> >");
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ModuleDef)));
    }

    public override ExportFactory<Key32> CreateExportFactory()
    {
        return new ExportFactory<Key32>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad(),
            () => new ExportEntry32());
    }
}