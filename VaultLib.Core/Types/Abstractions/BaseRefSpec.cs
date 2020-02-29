// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/12/2019 @ 10:31 AM.

using System.Collections.Generic;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Abstractions
{
    public abstract class BaseRefSpec : VLTBaseType, IReferencesCollections
    {
        protected BaseRefSpec(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field,
            collection)
        {
        }

        protected BaseRefSpec(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public abstract string ClassKey { get; set; }
        public abstract string CollectionKey { get; set; }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            yield return new CollectionReferenceInfo(this,
                database.RowManager.FindCollectionByName(ClassKey, CollectionKey));
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return ClassKey == classKey && CollectionKey == collectionKey;
        }

        public override string ToString()
        {
            return $"{ClassKey} -> {CollectionKey}";
        }
    }
}