// This file is part of VaultLib.Support.Undercover by heyitsleo.
// 
// Created: 10/31/2019 @ 10:01 PM.

using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;
using VaultLib.Support.Undercover.VLT.Attrib.Query;

namespace VaultLib.Support.Undercover;

public class ModuleDef : BaseGameModule<Key32>
{
    public override void RegisterTypes(TypeRegistryBuilder<Key32> typeRegistry)
    {
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(typeof(ModuleDef).Assembly);
        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        typeRegistry.AddFieldOverride<CollectionNameToChildrenIndex>("aud_moment", "IndexTable_CollectionName");
        typeRegistry.AddFieldOverride<CollectionNameToParentIndex>("speech", "IndexTable_CollectionName");
        typeRegistry.AddFieldOverride<CollectionNameToChildrenIndex>("traffic_engine", "IndexTable_CollectionName");
        typeRegistry.AddFieldOverride<CollectionNameToChildrenIndex>("traffic_horn", "IndexTable_CollectionName");
    }

    public override ExportFactory<Key32> CreateExportFactory()
    {
        return new ExportFactory<Key32>(() => new DatabaseLoad(), () => new ClassLoad32(), () => new CollectionLoad(),
            () => new ExportEntry32());
    }
}