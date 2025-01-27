// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 5:47 PM.

using System;
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
public class RowManager<TKey> where TKey : IKey<TKey>
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
    /// <param name="className"></param>
    /// <returns>The list of collections</returns>
    public List<VltCollection<TKey>> GetCollections(string className)
    {
        return Rows.FindAll(c => c.Class.Name == className);
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
    /// <param name="className">The name of the class to search in.</param>
    /// <returns>The collection enumerator.</returns>
    public IEnumerable<VltCollection<TKey>> EnumerateCollections(string className)
    {
        return Rows.Where(c => c.Class.Name == className);
    }

    /// <summary>
    ///     Finds a collection in the given class with the given name.
    /// </summary>
    /// <param name="className">The class name to search in</param>
    /// <param name="collectionName">The collection name to search for</param>
    /// <returns>The collection, if one is found, or null</returns>
    public VltCollection<TKey> FindCollectionByName(string className, string collectionName)
    {
        return EnumerateCollections(className).FirstOrDefault(collection => collection.Name == collectionName);
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
    public VltCollection<TKey> AddCollection(Vault<TKey> vault, string className, string newName, TKey key,
        VltCollection<TKey> parentCollection = null)
    {
        if (FindCollectionByName(className, newName) != null)
            throw new DuplicateNameException(
                $"A collection in the class '{className}' with the name '{newName}' already exists.");

        var collection = new VltCollection<TKey>(vault, _database.FindClass(className), newName, key);

        parentCollection?.AddChild(collection);
        Rows.Add(collection);

        return collection;
    }

    /// <summary>
    /// Manually adds a collection to the list of collections
    /// </summary>
    /// <param name="collection">The collection to add</param>
    /// <param name="check"></param>
    public void AddCollection(VltCollection<TKey> collection, bool check = false)
    {
        if (check && Rows.Any(r => r.ShortPath == collection.ShortPath))
            throw new Exception(
                $"Collection '{collection.ShortPath}' has already been added. Did you mean to add a clone with a new name?");

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