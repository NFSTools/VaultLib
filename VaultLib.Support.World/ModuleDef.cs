// This file is part of VaultLib.Support.World by heyitsleo.
// 
// Created: 11/02/2019 @ 1:32 PM.

using System;
using System.Reflection;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;
using VaultLib.ModernBase;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;
using VaultLib.Support.World.VLT.Attrib.Query;

namespace VaultLib.Support.World;

public class ModuleDef : BaseGameModule<Key32>
{
    public override void RegisterTypes(TypeRegistryBuilder<Key32> typeRegistry)
    {
        SpeedFramework.Register(typeRegistry);
        typeRegistry.RegisterAssemblyTypes(typeof(ModuleDef).Assembly);

        typeRegistry.Register<StringKey32>("Attrib::StringKey");
        typeRegistry.AddFieldOverride<CollectionNameToChildrenIndex>(
            "aud_moment", "IndexTable_CollectionName");
        typeRegistry.AddFieldOverride<CollectionNameToChildrenIndex>(
            "traffic_engine", "IndexTable_CollectionName");
        typeRegistry.AddFieldOverride<CollectionNameToChildrenIndex>(
            "traffic_horn", "IndexTable_CollectionName");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "AmountAlternateLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "AmountLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "CategoryLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "DescriptionLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "EventCategoryLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "EventModeDescriptionLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "EventModeLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "Localization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "NameLocalization");
        typeRegistry.AddFieldOverride<BinKey32>("gameplay", "RewardModeLocalization");

        typeRegistry.RegisterPrimitive("DUMMY_DateTime", br => new DateTime(br.ReadInt64()),
            (dt, w) => w.Write(dt.Ticks));
        typeRegistry.AddFieldOverride<DateTime>("gameplay", "StartDateTime");
        typeRegistry.AddFieldOverride<DateTime>("gameplay", "EndDateTime");
    }

    public override ExportFactory<Key32> CreateExportFactory()
    {
        var exportFactory = new ExportFactory<Key32>(() => new DatabaseLoad(), () => new ClassLoad32(),
            () => new CollectionLoad(),
            () => new ExportEntry32());
        exportFactory.RegisterExportType<VaultSlotExport<Key32>>(Key32.FromString("VaultDataType"));

        return exportFactory;
    }
}