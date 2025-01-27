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

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo(nameof(EffectLinkageRecord))]
public class EffectLinkageRecord: VltBaseType<uint>, IReferencesCollections<uint>
{
    public RefSpec<uint> Surface { get; set; } = new();
    public RefSpec<uint> Effect { get; set; } = new();
    public float MinSpeed { get; set; }
    public float MaxSpeed { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Surface.Read(context, fieldContext, br);
        Effect.Read(context, fieldContext, br);

        MinSpeed = br.ReadSingle();
        MaxSpeed = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        Surface.Write(context, fieldContext, bw);
        Effect.Write(context, fieldContext, bw);
        bw.Write(MinSpeed);
        bw.Write(MaxSpeed);
    }

    public IEnumerable<CollectionReferenceInfo<uint>> GetReferencedCollections(Database<uint> database, Vault<uint> vault)
    {
        return Surface.GetReferencedCollections(database, vault)
            .Concat(Effect.GetReferencedCollections(database, vault));
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        return Surface.ReferencesCollection(classKey, collectionKey)
               || Effect.ReferencesCollection(classKey, collectionKey);
    }
}