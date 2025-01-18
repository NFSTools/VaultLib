// This file is part of MostWantedSDK by heyitsleo.
// 
// Created: 10/20/2019 @ 9:05 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(AICollisionReactionRecord))]
    public class AICollisionReactionRecord : VltBaseType, IReferencesCollections
    {
        public uint Goal { get; set; }

        public RefSpec Reaction { get; set; } = new();

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Goal = br.ReadUInt32();
            Reaction.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Goal);
            Reaction.Write(context, fieldContext, bw);
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            throw new System.NotImplementedException();
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            throw new System.NotImplementedException();
        }
    }
}