// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/13/2019 @ 10:07 AM.

using System.Collections.Generic;
using VaultLib.Core.DB;

namespace VaultLib.Core.Utils
{
    public interface IReferencesCollections
    {
        IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault);

        bool ReferencesCollection(string classKey, string collectionKey);
    }
}