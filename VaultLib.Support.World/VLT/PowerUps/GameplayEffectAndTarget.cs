// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:32 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.PowerUps;

[VltTypeInfo("PowerUps::GameplayEffectAndTarget")]
public class GameplayEffectAndTarget: VltBaseType<uint>, IReferencesCollections<uint>
{
    public string GroupKey { get; set; }
    public uint Type { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        var rs = new RefSpec<uint>();
        rs.Read(context, fieldContext, br);
        uint type = br.ReadUInt32();

        GroupKey = rs.CollectionKey;
        Type = type;
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        var rs = new RefSpec<uint>();
        rs.ClassKey = "powerup_gamegroup";
        rs.CollectionKey = GroupKey;
        rs.Write(context, fieldContext, bw);
        bw.Write(Type);
    }

    public IEnumerable<CollectionReferenceInfo<uint>> GetReferencedCollections(Database<uint> database, Vault<uint> vault)
    {
        yield return new CollectionReferenceInfo<uint>(this,
            database.RowManager.FindCollectionByName("powerup_gamegroup", GroupKey));
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        return classKey == "powerup_gamegroup" && collectionKey == GroupKey;
    }
}