// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:22 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(TrafficPatternRecord))]
    public class TrafficPatternRecord : VLTBaseType, IReferencesCollections
    {
        public RefSpec Vehicle { get; set; }
        public float Rate { get; set; }
        public uint MaxInstances { get; set; }
        public uint Percent { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Vehicle = new RefSpec(Class, Field, Collection);
            Vehicle.Read(vault, br);
            Rate = br.ReadSingle();
            MaxInstances = br.ReadUInt32();
            Percent = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Vehicle.Write(vault, bw);
            bw.Write(Rate);
            bw.Write(MaxInstances);
            bw.Write(Percent);
        }

        public override string ToString()
        {
            return $"Vehicle: {Vehicle} | Spawn rate: {Rate} | Instances: {MaxInstances} | {Percent}%";
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            return Vehicle.GetReferencedCollections(database, vault);
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return Vehicle.ClassKey == classKey && Vehicle.CollectionKey == collectionKey;
        }

        public TrafficPatternRecord(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public TrafficPatternRecord(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}