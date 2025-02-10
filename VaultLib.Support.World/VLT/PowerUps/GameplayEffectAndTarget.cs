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
public class GameplayEffectAndTarget: VltBaseType<Key32>, IReferencesCollections<Key32>
{
    public Key32 GroupKey { get; set; }
    public uint Type { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryReader br)
    {
        var rs = new RefSpec32();
        rs.Read(context, fieldContext, br);
        uint type = br.ReadUInt32();

        GroupKey = rs.CollectionKey;
        Type = type;
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryWriter bw)
    {
        var rs = new RefSpec32();
        rs.ClassKey = Key32.FromString("powerup_gamegroup");
        rs.CollectionKey = GroupKey;
        rs.Write(context, fieldContext, bw);
        bw.Write(Type);
    }

    public IEnumerable<CollectionReferenceInfo<Key32>> GetReferencedCollections(Database<Key32> database, Vault<Key32> vault)
    {
        yield return new CollectionReferenceInfo<Key32>(this,
            database.RowManager.FindCollection(Key32.FromString("powerup_gamegroup"), GroupKey));
    }

    public bool ReferencesCollection(Key32 classKey, Key32 collectionKey)
    {
        return classKey == Key32.FromString("powerup_gamegroup") && collectionKey == GroupKey;
    }

    public override object Clone()
    {
        return MemberwiseClone();
    }
}