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
public class GameplayEffectAndTarget : VltBaseType, IReferencesCollections
{
    public string GroupKey { get; set; }
    public uint Type { get; set; }

    public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
    {
        RefSpec rs = new RefSpec();
        rs.Read(context, fieldContext, br);
        uint type = br.ReadUInt32();

        GroupKey = rs.CollectionKey;
        Type = type;
    }

    public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
    {
        RefSpec rs = new RefSpec();
        rs.ClassKey = "powerup_gamegroup";
        rs.CollectionKey = GroupKey;
        rs.Write(context, fieldContext, bw);
        bw.Write(Type);
    }

    public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
    {
        yield return new CollectionReferenceInfo(this,
            database.RowManager.FindCollectionByName("powerup_gamegroup", GroupKey));
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        return classKey == "powerup_gamegroup" && collectionKey == GroupKey;
    }
}