// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 4:14 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(AICollisionReactionRecord))]
    public class AICollisionReactionRecord : VLTBaseType, IReferencesCollections
    {
        public uint Goal { get; set; }
        public RefSpec Reaction { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Goal = br.ReadUInt32();
            Reaction = new RefSpec(Class, Field, Collection);
            Reaction.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Goal);
            Reaction.Write(vault, bw);
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            return Reaction.GetReferencedCollections(database, vault);
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return Reaction.ClassKey == classKey && Reaction.CollectionKey == collectionKey;
        }

        public AICollisionReactionRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public AICollisionReactionRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}