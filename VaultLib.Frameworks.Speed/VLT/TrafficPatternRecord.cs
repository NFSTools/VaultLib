// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/29/2019 @ 11:22 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(TrafficPatternRecord))]
    public class TrafficPatternRecord : VltBaseType, IReferencesCollections
    {
        public RefSpec Vehicle { get; set; } = new();
        public float Rate { get; set; }
        public uint MaxInstances { get; set; }
        public uint Percent { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Vehicle.Read(context, fieldContext, br);
            Rate = br.ReadSingle();
            MaxInstances = br.ReadUInt32();
            Percent = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Vehicle.Write(context, fieldContext, bw);
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
    }
}