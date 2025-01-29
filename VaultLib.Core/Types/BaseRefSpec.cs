// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/12/2019 @ 10:31 AM.

using System.Collections.Generic;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public abstract class BaseRefSpec<TKey> : VltBaseType<TKey>, IReferencesCollections<TKey>
    where TKey : struct, IKey<TKey>
{
    public abstract TKey GetClassKey();
    public abstract TKey GetCollectionKey();
    public abstract void SetClassKey(TKey classKey);
    public abstract void SetCollectionKey(TKey collectionKey);

    public IEnumerable<CollectionReferenceInfo<TKey>> GetReferencedCollections(Database<TKey> database,
        Vault<TKey> vault)
    {
        yield return new CollectionReferenceInfo<TKey>(this,
            database.RowManager.FindCollection(GetClassKey(), GetCollectionKey()));
    }

    public bool ReferencesCollection(TKey classKey, TKey collectionKey)
    {
        return GetClassKey() == classKey && GetCollectionKey() == collectionKey;
    }

    public override string ToString()
    {
        return $"{GetClassKey()} -> {GetCollectionKey()}";
    }
}