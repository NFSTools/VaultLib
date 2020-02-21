// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 9:35 AM.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(EffectLinkageRecord))]
    public class EffectLinkageRecord : VLTBaseType, IReferencesCollections
    {
        public RefSpec Surface { get; set; }
        public RefSpec Effect { get; set; }
        public float MinSpeed { get; set; }
        public float MaxSpeed { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Surface = new RefSpec(Class, Field, Collection);
            Effect = new RefSpec(Class, Field, Collection);

            Surface.Read(vault, br);
            Effect.Read(vault, br);

            MinSpeed = br.ReadSingle();
            MaxSpeed = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Surface.Write(vault, bw);
            Effect.Write(vault, bw);
            bw.Write(MinSpeed);
            bw.Write(MaxSpeed);
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            return Surface.GetReferencedCollections(database, vault)
                .Concat(Effect.GetReferencedCollections(database, vault));
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return Surface.ReferencesCollection(classKey, collectionKey)
                   || Effect.ReferencesCollection(classKey, collectionKey);
        }

        public EffectLinkageRecord(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public EffectLinkageRecord(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}