// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 5:47 PM.

using System.Collections.Generic;
using System.Data;
using System.Linq;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;

namespace VaultLib.Core;

/// <summary>
///     Manager class for collections ("rows")
///     Provides methods to access and manage row data
/// </summary>
public class RowManager<TKey> where TKey : struct, IKey<TKey>
{
    private readonly Database<TKey> _database;

    internal List<VltCollection<TKey>> Rows { get; }

    public RowManager(Database<TKey> database)
    {
        _database = database;
        Rows = new List<VltCollection<TKey>>();
    }

    /// <summary>
    /// Provides an enumerator to access every collection in the database that is part of the given vault.
    /// </summary>
    /// <param name="vault">The vault to obtain collections for.</param>
    /// <returns>A collection enumerator</returns>
    public IEnumerable<VltCollection<TKey>> GetCollectionsInVault(Vault<TKey> vault)
    {
        return Rows.Where(c => c.Vault == vault);
    }

    /// <summary>
    ///     Gets a read-only list of all collections in the database.
    /// </summary>
    /// <returns>The list of collections</returns>
    public IReadOnlyList<VltCollection<TKey>> GetCollections()
    {
        return Rows;
    }

    /// <summary>
    ///     Builds a list of every collection associated with the specified class.
    /// </summary>
    /// <param name="classKey"></param>
    /// <returns>The list of collections</returns>
    public List<VltCollection<TKey>> GetCollections(TKey classKey)
    {
        return EnumerateCollections(classKey).ToList();
    }

    /// <summary>
    ///     Builds a list of every collection associated with the specified class.
    /// </summary>
    /// <param name="className"></param>
    /// <returns>The list of collections</returns>
    public List<VltCollection<TKey>> GetCollections(string className)
    {
        return GetCollections(TKey.FromString(className));
    }

    /// <summary>
    ///     Provides access to an enumerator of every collection in the database.
    ///     This is ideal for high-performance requirements.
    /// </summary>
    /// <returns>The collection enumerator.</returns>
    public IEnumerable<VltCollection<TKey>> EnumerateCollections()
    {
        return Rows;
    }

    /// <summary>
    ///     Provides access to an enumerator of every collection in the database that is part of a class.
    ///     This is ideal for high-performance requirements.
    /// </summary>
    /// <param name="classKey">The key of the class to search in.</param>
    /// <returns>The collection enumerator.</returns>
    public IEnumerable<VltCollection<TKey>> EnumerateCollections(TKey classKey)
    {
        return Rows.Where(c => c.Class.Key == classKey);
    }

    /// <summary>
    ///     Provides access to an enumerator of every collection in the database that is part of a class.
    ///     This is ideal for high-performance requirements.
    /// </summary>
    /// <param name="className">The name of the class to search in.</param>
    /// <returns>The collection enumerator.</returns>
    public IEnumerable<VltCollection<TKey>> EnumerateCollections(string className)
    {
        return EnumerateCollections(TKey.FromString(className));
    }

    /// <summary>
    ///     Finds a collection in the given class with the given name.
    /// </summary>
    /// <param name="classKey">The class name to search in</param>
    /// <param name="collectionKey">The collection name to search for</param>
    /// <returns>The collection, if one is found, or null</returns>
    public VltCollection<TKey>? FindCollection(TKey classKey, TKey collectionKey)
    {
        return EnumerateCollections(classKey).FirstOrDefault(collection => collection.Key == collectionKey);
    }

    /// <summary>
    ///     Finds a collection in the given class with the given name.
    /// </summary>
    /// <param name="className">The class name to search in</param>
    /// <param name="collectionName">The collection name to search for</param>
    /// <returns>The collection, if one is found, or null</returns>
    public VltCollection<TKey>? FindCollection(string className, string collectionName)
    {
        return FindCollection(TKey.FromString(className), TKey.FromString(collectionName));
    }

    /// <summary>
    ///     Adds a collection with the given name to the given class, optionally making it
    ///     the child of the given parent collection.
    /// </summary>
    /// <param name="vault">The vault to add the collection to.</param>
    /// <param name="classKey">The key of the class to add the collection to.</param>
    /// <param name="key">The collection's key.</param>
    /// <param name="parentCollection">The parent collection, if one is necessary.</param>
    /// <returns>The new collection</returns>
    public VltCollection<TKey> AddCollection(Vault<TKey> vault, TKey classKey, TKey key,
        VltCollection<TKey>? parentCollection = null)
    {
        if (FindCollection(classKey, key) != null)
            throw new DuplicateNameException("The specified key is already in use by another collection");

        var collection = new VltCollection<TKey>(vault, _database.FindClass(classKey), key);

        collection.SetParent(parentCollection);
        // parentCollection?.AddChild(collection);
        Rows.Add(collection);

        return collection;
    }

    /// <summary>
    ///     Adds a collection with the given name to the given class, optionally making it
    ///     the child of the given parent collection.
    /// </summary>
    /// <param name="vault">The vault to add the collection to.</param>
    /// <param name="className">The name of the class to add the collection to.</param>
    /// <param name="name">The name of the collection.</param>
    /// <param name="parentCollection">The parent collection, if one is necessary.</param>
    /// <returns>The new collection</returns>
    public VltCollection<TKey> AddCollection(Vault<TKey> vault, string className, string name,
        VltCollection<TKey>? parentCollection = null)
    {
        return AddCollection(vault, TKey.FromString(className), TKey.FromString(name), parentCollection);
    }

    /// <summary>
    /// Manually adds a collection to the list of collections
    /// </summary>
    /// <param name="collection">The collection to add</param>
    public void AddCollection(VltCollection<TKey> collection)
    {
        // TODO: bring this check back, maybe behind an option?
        // if (Rows.Any(r => r.Class.Key == collection.Class.Key && r.Key == collection.Key))
        //     throw new DuplicateNameException("The specified key is already in use by another collection");

        Rows.Add(collection);
    }

    /// <summary>
    /// Removes a collection from the list of collections.
    /// </summary>
    /// <param name="collection">The collection to remove</param>
    public void RemoveCollection(VltCollection<TKey> collection)
    {
        Rows.Remove(collection);
    }
}