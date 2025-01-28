// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/12/2019 @ 10:31 AM.

using System.Collections.Generic;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Abstractions;

public abstract class BaseRefSpec<TKey> : VltBaseType<TKey>, IReferencesCollections<TKey> where TKey : struct, IKey<TKey>
{
    public abstract TKey ClassKey { get; set; }
    public abstract TKey CollectionKey { get; set; }

    public IEnumerable<CollectionReferenceInfo<TKey>> GetReferencedCollections(Database<TKey> database,
        Vault<TKey> vault)
    {
        yield return new CollectionReferenceInfo<TKey>(this,
            database.RowManager.FindCollection(ClassKey, CollectionKey));
    }

    public bool ReferencesCollection(TKey classKey, TKey collectionKey)
    {
        return ClassKey == classKey && CollectionKey == collectionKey;
    }

    public override string ToString()
    {
        return $"{ClassKey} -> {CollectionKey}";
    }
}