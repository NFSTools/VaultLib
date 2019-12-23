// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:33 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.PowerUps
{
    [VLTTypeInfo("PowerUps::EmitterEffectAndTarget")]
    public class EmitterEffectAndTarget : VLTBaseType, IReferencesCollections
    {
        public string EmitterKey { get; set; }
        public uint Type { get; set; }
        public float Intensity { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            RefSpec rs = new RefSpec(Class, Field, Collection);
            rs.Read(vault, br);

            EmitterKey = rs.CollectionKey;
            Type = br.ReadUInt32();
            Intensity = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            RefSpec rs = new RefSpec(Class, Field, Collection);
            rs.ClassKey = "emittergroup";
            rs.CollectionKey = EmitterKey;
            rs.Write(vault, bw);
            bw.Write(Type);
            bw.Write(Intensity);
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            yield return new CollectionReferenceInfo(this, database.RowManager.FindCollectionByName("emittergroup", EmitterKey));
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return classKey == "emittergroup" && collectionKey == EmitterKey;
        }

        public EmitterEffectAndTarget(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public EmitterEffectAndTarget(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}