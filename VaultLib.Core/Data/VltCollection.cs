// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:04 PM.

using System;
using System.Collections.Generic;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Types;

namespace VaultLib.Core.Data;

/// <summary>
///     A collection in VLT is like a row in a SQL database.
///     A collection specifies values for the fields of its class.
/// </summary>
public class VltCollection<TKey> where TKey : struct, IKey<TKey>
{
    /// <summary>
    /// Gets the class that the collection belongs to.
    /// </summary>
    public VltClass<TKey> Class { get; }

    /// <summary>
    /// Gets or sets the vault that the collection belongs to.
    /// </summary>
    public Vault<TKey> Vault { get; private set; }

    public TKey Key { get; private set; }

    /// <summary>
    /// Gets the collection's parent.
    /// </summary>
    public VltCollection<TKey>? Parent { get; private set; }

    /// <summary>
    /// Gets the collection's data table.
    /// </summary>
    private VltDataTable<TKey> Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VltCollection{TKey}"/> class.
    /// </summary>
    /// <param name="vault">The vault that contains the collection.</param>
    /// <param name="vltClass">The class that the collection is part of.</param>
    /// <param name="key">The collection's unique key.</param>
    public VltCollection(Vault<TKey> vault, VltClass<TKey> vltClass, TKey key)
    {
        Vault = vault;
        Class = vltClass;
        Key = key;
        Data = new VltDataTable<TKey>();
    }

    #region API Members

    public void SetParent(VltCollection<TKey>? parent)
    {
        if (parent != null)
        {
            if (!ReferenceEquals(parent.Class, Class))
            {
                throw new ArgumentException("New parent collection belongs to a different class.");
            }
        }
        
        Parent = parent;
    }

    /// <summary>
    /// Changes the vault that the collection is associated with.
    /// </summary>
    /// <param name="vault">The new parent vault.</param>
    public void SetVault(Vault<TKey> vault)
    {
        Vault = vault;
    }

    public void SetKey(TKey newKey)
    {
        if (Key == newKey)
            return;

        if (Vault.Database.RowManager.FindCollection(Class.Key, newKey) != null)
        {
            throw new ArgumentException($"A collection with the same key ({newKey}) already exists", nameof(newKey));
        }

        Key = newKey;
    }

    /// <summary>
    /// Gets a read-only copy of the collection's data dictionary.
    /// </summary>
    /// <remarks>This method does not perform any conversions. It returns the underlying objects for everything.</remarks>
    /// <returns>The read-only data dictionary.</returns>
    public IReadOnlyDictionary<TKey, object> GetData()
    {
        return Data.GetDictionary();
    }

    public IReadOnlyList<VltDataTable<TKey>.Entry> GetOrderedData()
    {
        return Data.GetEntries();
    }

    /// <summary>
    /// Determines if the collection has a data entry with the given key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns><c>true</c> if an entry exists; otherwise, <c>false</c>.</returns>
    public bool HasEntry(TKey key) => Data.HasValue(key);

    /// <summary>
    /// Determines if the collection's data table has a value associated
    /// with a particular field.
    /// </summary>
    /// <param name="name">The field's name.</param>
    /// <returns><c>true</c> if an entry exists; otherwise, <c>false</c>.</returns>
    public bool HasEntry(string name) => HasEntry(TKey.FromString(name));

    /// <summary>
    /// Gets the value for a particular field from the collection's data table.
    /// </summary>
    /// <param name="key">The field's key.</param>
    /// <returns>The data mapped to the given key.</returns>
    /// <exception cref="KeyNotFoundException">If there is no value mapped to the given key.</exception>
    public object GetRawValue(TKey key)
    {
        return GetRawValue<object>(key);
    }

    /// <summary>
    /// Gets the value for a particular field from the collection's data table.
    /// </summary>
    /// <param name="name">The name of the field to obtain the value of.</param>
    /// <returns>The data mapped to the given key.</returns>
    /// <exception cref="KeyNotFoundException">If there is no value mapped to the given key.</exception>
    public object GetRawValue(string name)
    {
        return GetRawValue<object>(TKey.FromString(name));
    }

    /// <summary>
    /// Gets the value for a particular field from the collection's data table.
    /// </summary>
    /// <param name="key">The field's key.</param>
    /// <returns>The data mapped to the given key.</returns>
    /// <exception cref="KeyNotFoundException">If there is no value mapped to the given key.</exception>
    public T GetRawValue<T>(TKey key)
    {
        if (!Data.TryGetValue<T>(key, out var data))
            throw new KeyNotFoundException($"Collection does not have a value for field {key}");
        return data;
    }

    /// <summary>
    /// Gets the value for a particular field from the collection's data table.
    /// </summary>
    /// <param name="name">The name of the field to obtain the value of.</param>
    /// <returns>The data mapped to the given key.</returns>
    /// <exception cref="KeyNotFoundException">If there is no value mapped to the given key.</exception>
    public T GetRawValue<T>(string name)
    {
        return GetRawValue<T>(TKey.FromString(name));
    }

    /// <summary>
    /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="key"/> in the collection's data dictionary.
    /// </summary>
    /// <typeparam name="T">The data type to be obtained.</typeparam>
    /// <param name="key">The mapping key.</param>
    /// <param name="index">The array index to retrieve the value from.</param>
    /// <returns>The mapping value.</returns>
    public T GetRawValue<T>(TKey key, int index)
    {
        var data = GetRawValue(key, index);

        if (data is not T value)
            throw new InvalidCastException($"Field {key} is not compatible with type {typeof(T)}");

        return value;
    }

    /// <summary>
    /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="name"/> in the collection's data dictionary.
    /// </summary>
    /// <typeparam name="T">The data type to be obtained.</typeparam>
    /// <param name="name">The mapping key.</param>
    /// <param name="index">The array index to retrieve the value from.</param>
    /// <returns>The mapping value.</returns>
    public T GetRawValue<T>(string name, int index)
    {
        return GetRawValue<T>(TKey.FromString(name), index);
    }

    public object GetRawValue(TKey key, int index)
    {
        var array = GetRawValue<VltArrayType<TKey>>(key);

        if (index < 0 || index >= array.Items.Count)
        {
            throw new ArgumentException($"Failed condition: 0 <= {index} < {array.Items.Count}");
        }

        return array.Items[index];
    }

    public object GetRawValue(string name, int index)
    {
        return GetRawValue(TKey.FromString(name), index);
    }

    /// <summary>
    /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    /// </summary>
    /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    /// <param name="data">The mapping value.</param>
    public void SetRawValue(TKey key, object data)
    {
        if (Class.HasField(key))
        {
            Data.SetValue(key, data);
        }
        else
        {
            throw new KeyNotFoundException($"Field '{key}' not found in class");
        }
    }

    /// <summary>
    /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    /// </summary>
    /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    /// <param name="data">The mapping value.</param>
    public void SetRawValue(string key, object data)
    {
        SetRawValue(TKey.FromString(key), data);
    }

    /// <summary>
    /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    /// </summary>
    /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    /// <param name="index"></param>
    /// <param name="data">The mapping value.</param>
    public void SetRawValue<T>(TKey key, int index, T data) where T : notnull
    {
        var array = GetRawValue<VltArrayType<TKey>>(key);

        if (index < 0 || index >= array.Items.Count)
        {
            throw new ArgumentException($"Failed condition: 0 <= {index} < {array.Items.Count}");
        }

        if (data.GetType() != array.ItemType)
        {
            throw new ArgumentException($"Type mismatch: T={data.GetType()} A={array.ItemType}");
        }

        array.Items[index] = data;
    }

    /// <summary>
    /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    /// </summary>
    /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    /// <param name="index"></param>
    /// <param name="data">The mapping value.</param>
    public void SetRawValue<T>(string key, int index, T data) where T : notnull
    {
        SetRawValue(TKey.FromString(key), index, data);
    }

    /// <summary>
    /// Removes an entry from the data dictionary.
    /// This is only valid for optional fields.
    /// </summary>
    /// <param name="key">The mapping key.</param>
    public void RemoveValue(TKey key)
    {
        if (Class.HasField(key))
        {
            var field = Class[key];

            if (field.IsInLayout)
            {
                throw new Exception($"Cannot remove in-layout field: {key}");
            }

            if (HasEntry(key))
            {
                Data.RemoveValue(key);
            }
            else
            {
                throw new KeyNotFoundException($"Collection does not have an entry for '{key}'");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Class does not have field '{key}'");
        }
    }

    /// <summary>
    /// Removes an entry from the data dictionary.
    /// This is only valid for optional fields.
    /// </summary>
    /// <param name="name">The mapping key.</param>
    public void RemoveValue(string name)
    {
        RemoveValue(TKey.FromString(name));
    }

    #endregion

    #region Internal stuff

    // private object BaseTypeToData(VltBaseType baseType)
    // {
    //     // if we have a primitive or string value, return that
    //     // if we have an array, return a list where each item in the array has been converted (recursion FTW)
    //     // otherwise, just return the original data
    //
    //     return baseType switch
    //     {
    //         PrimitiveTypeBase ptb => ptb.GetValue(),
    //         IStringValue sv => sv.GetString(),
    //         VltArrayType array => array.Items.Select(BaseTypeToData).ToList(),
    //         _ => baseType
    //     };
    // }

    // private VltBaseType DataToBaseType(VltClassField field, VltBaseType originalData, object data)
    // {
    //     switch (data)
    //     {
    //         case string s:
    //         {
    //             if (originalData is IStringValue sv)
    //             {
    //                 sv.SetString(s);
    //                 return originalData;
    //             }
    //
    //             break;
    //         }
    //         case IConvertible ic:
    //         {
    //             if (originalData is PrimitiveTypeBase ptb)
    //             {
    //                 ptb.SetValue(ic);
    //                 return originalData;
    //             }
    //
    //             break;
    //         }
    //         case VltBaseType vbt:
    //             return vbt;
    //     }
    //
    //     throw new ArgumentException($"Cannot convert {data.GetType()} to VLTBaseType.");
    // }

    #endregion
}