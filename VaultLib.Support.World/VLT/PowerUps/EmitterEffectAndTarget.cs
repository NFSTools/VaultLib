// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 8:33 PM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.PowerUps;

[VltTypeInfo("PowerUps::EmitterEffectAndTarget")]
public class EmitterEffectAndTarget: VltBaseType<Key32>, IReferencesCollections<Key32>
{
    public string EmitterKey { get; set; }
    public uint Type { get; set; }
    public float Intensity { get; set; }

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryReader br)
    {
        var rs = new RefSpec<Key32>();
        rs.Read(context, fieldContext, br);

        EmitterKey = rs.CollectionKey;
        Type = br.ReadUInt32();
        Intensity = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext, BinaryWriter bw)
    {
        var rs = new RefSpec<Key32>();
        rs.ClassKey = "emittergroup";
        rs.CollectionKey = EmitterKey;
        rs.Write(context, fieldContext, bw);
        bw.Write(Type);
        bw.Write(Intensity);
    }

    public IEnumerable<CollectionReferenceInfo<Key32>> GetReferencedCollections(Database<Key32> database, Vault<Key32> vault)
    {
        yield return new CollectionReferenceInfo<Key32>(this,
            database.RowManager.FindCollection("emittergroup", EmitterKey));
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        return classKey == "emittergroup" && collectionKey == EmitterKey;
    }
}