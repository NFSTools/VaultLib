// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 5:47 PM.

using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Data;
using VaultLib.Core.DB;

namespace VaultLib.Core
{
    /// <summary>
    /// Manager class for collections ("rows")
    /// Provides methods to access and manage row data
    /// </summary>
    public class RowManager
    {
        private readonly Database _database;

        public RowManager(Database database)
        {
            _database = database;
        }

        /// <summary>
        /// Builds a list of every collection, parent or child, in the database.
        /// </summary>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The list of collections</returns>
        public List<VLTCollection> GetFlattenedCollections(IEnumerable<VLTCollection> collections = null)
        {
            if (collections == null)
                collections = _database.Rows;
            List<VLTCollection> list = new List<VLTCollection>();

            foreach (var vltCollection in collections)
            {
                list.Add(vltCollection);
                if (vltCollection.Children.Count > 0)
                {
                    list.AddRange(GetFlattenedCollections(vltCollection.Children));
                }
            }

            return list;
        }

        /// <summary>
        /// Builds a list of every collection, parent or child, in the database.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The list of collections</returns>
        public List<VLTCollection> GetFlattenedCollections(string className, IEnumerable<VLTCollection> collections = null)
        {
            if (collections == null)
                collections = _database.Rows.FindAll(c => c.ClassName == className);
            List<VLTCollection> list = new List<VLTCollection>();

            foreach (var vltCollection in collections)
            {
                list.Add(vltCollection);
                if (vltCollection.Children.Count > 0)
                {
                    list.AddRange(GetFlattenedCollections(vltCollection.Children));
                }
            }

            return list;
        }

        /// <summary>
        /// Provides access to an enumerator of every collection in the database.
        /// This is ideal for high-performance requirements.
        /// </summary>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The collection enumerator.</returns>
        public IEnumerable<VLTCollection> EnumerateFlattenedCollections(IEnumerable<VLTCollection> collections = null)
        {
            if (collections == null)
                collections = _database.Rows;

            foreach (var vltCollection in collections)
            {
                yield return vltCollection;

                foreach (var collection in EnumerateFlattenedCollections(vltCollection.Children))
                {
                    yield return collection;
                }
            }
        }

        /// <summary>
        /// Provides access to an enumerator of every collection in the database that is part of a class.
        /// This is ideal for high-performance requirements.
        /// </summary>
        /// <param name="className">The name of the class to search in.</param>
        /// <param name="collections">For recursion purposes - the enumerator to obtain data from</param>
        /// <returns>The collection enumerator.</returns>
        public IEnumerable<VLTCollection> EnumerateFlattenedCollections(string className, IEnumerable<VLTCollection> collections = null)
        {
            if (collections == null)
                collections = _database.Rows.Where(c => c.ClassName == className);

            foreach (var vltCollection in collections)
            {
                yield return vltCollection;

                foreach (var collection in EnumerateFlattenedCollections(vltCollection.Children))
                {
                    yield return collection;
                }
            }
        }

        /// <summary>
        /// Finds a collection in the given class with the given name.
        /// </summary>
        /// <param name="className">The class name to search in</param>
        /// <param name="collectionName">The collection name to search for</param>
        /// <returns>The collection, if one is found, or null</returns>
        public VLTCollection FindCollectionByName(string className, string collectionName)
        {
            return (from vltCollection in EnumerateFlattenedCollections(className)
                    select vltCollection).FirstOrDefault(collection => collection.Name == collectionName);
        }

        /// <summary>
        /// Adds a collection with the given name to the given class, optionally making it
        /// the child of the given parent collection.
        /// </summary>
        /// <param name="vault">The vault to add the collection to.</param>
        /// <param name="className">The name of the class to add the collection to.</param>
        /// <param name="newName">The name of the collection.</param>
        /// <param name="parentCollection">The parent collection, if one is necessary.</param>
        /// <returns>The new collection</returns>
        public VLTCollection AddCollection(Vault vault, string className, string newName, VLTCollection parentCollection = null)
        {
            if (FindCollectionByName(className, newName) != null)
            {
                throw new DuplicateNameException($"A collection in the class '{className}' with the name '{newName}' already exists.");
            }

            VLTCollection collection = new VLTCollection(vault, _database.FindClass(className), newName,
                _database.Is64Bit ? VLT64Hasher.Hash(newName) : VLT32Hasher.Hash(newName));

            if (parentCollection != null)
            {
                // Make the new collection a child of the parent
                collection.SetParent(parentCollection);
            }
            else
            {
                // Just add the collection
                _database.Rows.Add(collection);
            }

            return collection;
        }
    }
}