// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:33 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.PowerUps;

[VltTypeInfo("PowerUps::EmitterEffectAndTarget")]
public class EmitterEffectAndTarget: VltBaseType<uint>, IReferencesCollections<uint>
{
    public string EmitterKey { get; set; }
    public uint Type { get; set; }
    public float Intensity { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        var rs = new RefSpec<uint>();
        rs.Read(context, fieldContext, br);

        EmitterKey = rs.CollectionKey;
        Type = br.ReadUInt32();
        Intensity = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        var rs = new RefSpec<uint>();
        rs.ClassKey = "emittergroup";
        rs.CollectionKey = EmitterKey;
        rs.Write(context, fieldContext, bw);
        bw.Write(Type);
        bw.Write(Intensity);
    }

    public IEnumerable<CollectionReferenceInfo<uint>> GetReferencedCollections(Database<uint> database, Vault<uint> vault)
    {
        yield return new CollectionReferenceInfo<uint>(this,
            database.RowManager.FindCollectionByName("emittergroup", EmitterKey));
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        return classKey == "emittergroup" && collectionKey == EmitterKey;
    }
}