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

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("AICollisionReactionRecord")]
public class AICollisionReactionRecord : VltBaseType<uint>, IReferencesCollections<uint>
{
    public uint Goal { get; set; }

    public RefSpec<uint> Reaction { get; set; } = new();

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Goal = br.ReadUInt32();
        Reaction.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        bw.Write(Goal);
        Reaction.Write(context, fieldContext, bw);
    }

    public IEnumerable<CollectionReferenceInfo<uint>> GetReferencedCollections(Database<uint> database, Vault<uint> vault)
    {
        throw new System.NotImplementedException();
    }

    public bool ReferencesCollection(string classKey, string collectionKey)
    {
        throw new System.NotImplementedException();
    }
}