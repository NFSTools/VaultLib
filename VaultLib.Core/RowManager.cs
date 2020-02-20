// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 5:47 PM.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VaultLib.Core.Data;
using VaultLib.Core.DB;

namespace VaultLib.Core
{
    /// <summary>
    ///     Manager class for collections ("rows")
    ///     Provides methods to access and manage row data
    /// </summary>
    public class RowManager
    {
        private readonly Database _database;

        public List<VltCollection> Rows { get; }

        public RowManager(Database database)
        {
            _database = database;
            Rows = new List<VltCollection>();
        }

        /// <summary>
        /// Provides an enumerator to access every collection in the database that is part of the given vault.
        /// </summary>
        /// <param name="vault">The vault to obtain collections for.</param>
        /// <returns>A collection enumerator</returns>
        public IEnumerable<VltCollection> GetCollectionsInVault(Vault vault)
        {
            return EnumerateFlattenedCollections().Where(c => c.Vault == vault);
        }

        /// <summary>
        /// Provides an enumerator to access every top-level collection in the database that is part of the given vault.
        /// </summary>
        /// <param name="vault">The vault to obtain collections for.</param>
        /// <returns>A collection enumerator</returns>
        public IEnumerable<VltCollection> GetTopCollectionsInVault(Vault vault)
        {
            return Rows.Where(c => c.Vault == vault);
        }

        /// <summary>
        ///     Builds a list of every collection, parent or child, in the database.
        /// </summary>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The list of collections</returns>
        public List<VltCollection> GetFlattenedCollections(IEnumerable<VltCollection> collections = null)
        {
            if (collections == null)
                collections = Rows;
            var list = new List<VltCollection>();

            foreach (var vltCollection in collections)
            {
                list.Add(vltCollection);
                if (vltCollection.Children.Count > 0) list.AddRange(GetFlattenedCollections(vltCollection.Children));
            }

            return list;
        }

        /// <summary>
        ///     Builds a list of every collection, parent or child, in the database.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The list of collections</returns>
        public List<VltCollection> GetFlattenedCollections(string className,
            IEnumerable<VltCollection> collections = null)
        {
            if (collections == null)
                collections = Rows.FindAll(c => c.Class.Name == className);
            var list = new List<VltCollection>();

            foreach (var vltCollection in collections)
            {
                list.Add(vltCollection);
                if (vltCollection.Children.Count > 0) list.AddRange(GetFlattenedCollections(vltCollection.Children));
            }

            return list;
        }

        /// <summary>
        ///     Provides access to an enumerator of every collection in the database.
        ///     This is ideal for high-performance requirements.
        /// </summary>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The collection enumerator.</returns>
        public IEnumerable<VltCollection> EnumerateFlattenedCollections(IEnumerable<VltCollection> collections = null)
        {
            if (collections == null)
                collections = Rows;

            foreach (var vltCollection in collections)
            {
                yield return vltCollection;

                foreach (var collection in EnumerateFlattenedCollections(vltCollection.Children))
                    yield return collection;
            }
        }

        /// <summary>
        ///     Provides access to an enumerator of every collection in the database that is part of a class.
        ///     This is ideal for high-performance requirements.
        /// </summary>
        /// <param name="className">The name of the class to search in.</param>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The collection enumerator.</returns>
        public IEnumerable<VltCollection> EnumerateFlattenedCollections(string className,
            IEnumerable<VltCollection> collections = null)
        {
            if (collections == null)
                collections = Rows.Where(c => c.Class.Name == className);

            foreach (var vltCollection in collections)
            {
                yield return vltCollection;

                foreach (var collection in EnumerateFlattenedCollections(vltCollection.Children))
                    yield return collection;
            }
        }

        /// <summary>
        ///     Provides access to an enumerator of every top-level collection in the database that is part of a class.
        ///     This is ideal for high-performance requirements.
        /// </summary>
        /// <param name="className">The name of the class to search in.</param>
        /// <returns>The collection enumerator.</returns>
        public IEnumerable<VltCollection> EnumerateCollections(string className)
        {
            return Rows.Where(c => c.Class.Name == className);
        }

        /// <summary>
        ///     Finds a collection in the given class with the given name.
        /// </summary>
        /// <param name="className">The class name to search in</param>
        /// <param name="collectionName">The collection name to search for</param>
        /// <returns>The collection, if one is found, or null</returns>
        public VltCollection FindCollectionByName(string className, string collectionName)
        {
            return (from vltCollection in EnumerateFlattenedCollections(className)
                    select vltCollection).FirstOrDefault(collection => collection.Name == collectionName);
        }

        /// <summary>
        ///     Adds a collection with the given name to the given class, optionally making it
        ///     the child of the given parent collection.
        /// </summary>
        /// <param name="vault">The vault to add the collection to.</param>
        /// <param name="className">The name of the class to add the collection to.</param>
        /// <param name="newName">The name of the collection.</param>
        /// <param name="parentCollection">The parent collection, if one is necessary.</param>
        /// <returns>The new collection</returns>
        public VltCollection AddCollection(Vault vault, string className, string newName,
            VltCollection parentCollection = null)
        {
            if (FindCollectionByName(className, newName) != null)
                throw new DuplicateNameException(
                    $"A collection in the class '{className}' with the name '{newName}' already exists.");

            var collection = new VltCollection(vault, _database.FindClass(className), newName);

            if (parentCollection != null)
                // Make the new collection a child of the parent
                parentCollection.AddChild(collection);
            else
                // Just add the collection
                Rows.Add(collection);

            return collection;
        }

        /// <summary>
        /// Manually adds a collection to the list of collections
        /// </summary>
        /// <param name="collection">The collection to add</param>
        /// <param name="check"></param>
        /// <returns>The added collection</returns>
        public VltCollection AddCollection(VltCollection collection, bool check = false)
        {
            if (check && Rows.Contains(collection))
                throw new Exception($"Collection '{collection.ShortPath}' has already been added. Did you mean to add a clone with a new name?");

            Rows.Add(collection);
            return collection;
        }

        /// <summary>
        /// Removes a collection from the list of collections.
        /// </summary>
        /// <param name="collection">The collection to remove</param>
        /// <exception cref="Exception">if the collection is not a top-level collection</exception>
        public void RemoveCollection(VltCollection collection)
        {
            if (!Rows.Contains(collection))
            {
                throw new Exception(
                    $"Collection '{collection.ShortPath}' is not a top-level collection. Did you mean to disassociate it from its parent?");
            }

            Rows.Remove(collection);
        }
    }
}