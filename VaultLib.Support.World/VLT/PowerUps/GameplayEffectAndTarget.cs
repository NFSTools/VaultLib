// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:32 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.PowerUps
{
    [VLTTypeInfo("PowerUps::GameplayEffectAndTarget")]
    public class GameplayEffectAndTarget : VLTBaseType, IReferencesCollections
    {
        public string GroupKey { get; set; }
        public uint Type { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            RefSpec rs = new RefSpec(Class, Field, Collection);
            rs.Read(vault, br);
            uint type = br.ReadUInt32();

            GroupKey = rs.CollectionKey;
            Type = type;
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            RefSpec rs = new RefSpec(Class, Field, Collection);
            rs.ClassKey = "powerup_gamegroup";
            rs.CollectionKey = GroupKey;
            rs.Write(vault, bw);
            bw.Write(Type);
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            yield return new CollectionReferenceInfo(this, database.RowManager.FindCollectionByName("powerup_gamegroup", GroupKey));
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return classKey == "powerup_gamegroup" && collectionKey == GroupKey;
        }

        public GameplayEffectAndTarget(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public GameplayEffectAndTarget(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}