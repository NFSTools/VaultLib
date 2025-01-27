// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:32 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.PowerUps;

[VltTypeInfo("PowerUps::GameplayEffectAndTarget")]
public class GameplayEffectAndTarget: VltBaseType<VaultLib.Core.DataInterfaces.Key32>, IReferencesCollections<VaultLib.Core.DataInterfaces.Key32>
{
    public string GroupKey { get; set; }
    public uint Type { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        var rs = new RefSpec<Key32>();
        rs.Read(context, fieldContext, br);
        uint type = br.ReadUInt32();

        GroupKey = rs.CollectionKey;
        Type = type;
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        var rs = new RefSpec<Key32>();
        rs.ClassKey = "powerup_gamegroup";
        rs.CollectionKey = GroupKey;
        rs.Write(context, fieldContext, bw);
        bw.Write(Type);
    }

    public IEnumerable<CollectionReferenceInfo<VaultLib.Core.DataInterfaces.Key32>> GetReferencedCollections(Database<VaultLib.Core.DataInterfaces.Key32> database, Vault<VaultLib.Core.DataInterfaces.Key32> vault)
    {
        yield return new CollectionReferenceInfo<VaultLib.Core.DataInterfaces.Key32>(this,
            database.RowManager.FindCollectionByName("powerup_gamegroup", GroupKey));
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        return classKey == "powerup_gamegroup" && collectionKey == GroupKey;
    }
}