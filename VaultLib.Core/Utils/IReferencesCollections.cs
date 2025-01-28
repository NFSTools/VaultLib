// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/13/2019 @ 10:07 AM.

using System.Collections.Generic;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;

namespace VaultLib.Core.Utils;

public interface IReferencesCollections<TKey> where TKey : struct, IKey<TKey>
{
    IEnumerable<CollectionReferenceInfo<TKey>> GetReferencedCollections(Database<TKey> database, Vault<TKey> vault);

    bool ReferencesCollection(TKey classKey, TKey collectionKey);
}