// This file is part of MostWantedSDK by heyitsleo.
// 
// Created: 10/20/2019 @ 9:05 PM.

using System;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.VLT;

[VltTypeInfo("AICollisionReactionRecord")]
public class AICollisionReactionRecord : VltBaseType<Key32>, IReferencesCollections<Key32>
{
    public Key32 Goal { get; set; }

    public RefSpec32 Reaction { get; set; } = new();

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        Goal = Key32.Read(br);
        Reaction.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        Goal.Write(bw);
        Reaction.Write(context, fieldContext, bw);
    }

    public IEnumerable<CollectionReferenceInfo<Key32>> GetReferencedCollections(Database<Key32> database,
        Vault<Key32> vault)
    {
        throw new NotImplementedException();
    }

    public bool ReferencesCollection(Key32 classKey, Key32 collectionKey)
    {
        throw new NotImplementedException();
    }

    public override object Clone()
    {
        return new AICollisionReactionRecord
        {
            Goal = Goal,
            Reaction = (RefSpec32)Reaction.Clone(),
        };
    }
}