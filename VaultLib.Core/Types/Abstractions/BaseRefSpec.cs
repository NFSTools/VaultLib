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
        public abstract string ClassKey { get; set; }
        public abstract string CollectionKey { get; set; }

        public virtual bool CanChangeClass => true;

        public override string ToString()
        {
            return $"{ClassKey} -> {CollectionKey}";
        }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            yield return new CollectionReferenceInfo(this, database.RowManager.FindCollectionByName(ClassKey, CollectionKey));
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return this.ClassKey == classKey && this.CollectionKey == collectionKey;
        }

        protected BaseRefSpec(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        protected BaseRefSpec(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}