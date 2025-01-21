// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:04 PM.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VaultLib.Core.Types;

namespace VaultLib.Core.Data;

/// <summary>
///     A collection in VLT is like a row in a SQL database.
///     A collection specifies values for the fields of its class.
/// </summary>
public class VltCollection
{
    /// <summary>
    /// Gets the <see cref="VltClass"/> that this collection is part of.
    /// </summary>
    public VltClass Class { get; }

    /// <summary>
    /// Gets or sets the <see cref="Core.Vault"/> that this collection is part of.
    /// </summary>
    public Vault Vault { get; private set; }

    /// <summary>
    /// Gets the name of this collection.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the parent collection of this collection.
    /// </summary>
    public VltCollection Parent { get; private set; }

    /// <summary>
    /// Gets the short path of the collection.
    /// </summary>
    /// <example>gameplay/baseelement</example>
    public string ShortPath => $"{Class.Name}/{Name}";

    /// <summary>
    /// Gets the collection's data.
    /// </summary>
    /// <remarks> This is a mapping between a <see cref="VltClassField"/>'s name and a <see cref="VltBaseType"/> instance.</remarks>
    private VltDataTable Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VltCollection"/> class.
    /// </summary>
    /// <param name="vault">The vault that contains the collection.</param>
    /// <param name="vltClass">The <see cref="VltClass"/> that the collection is part of.</param>
    /// <param name="name">The name of the collection.</param>
    public VltCollection(Vault vault, VltClass vltClass, string name)
    {
        Vault = vault;
        Class = vltClass;
        Name = name;
        Data = new VltDataTable();
    }

    #region API Members

    /// <summary>
    /// Updates the name of the collection.
    /// </summary>
    /// <param name="name"></param>
    /// <remarks>This method does not perform any validation. It is assumed that you know what you're doing!</remarks>
    public void SetName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Makes the current collection the parent of another collection.
    /// </summary>
    /// <param name="collection">The collection that is being made a child.</param>
    public void AddChild(VltCollection collection)
    {
        collection.Parent = this;
    }

    /// <summary>
    /// Breaks the parent-child relationship between the current collection and another collection.
    /// </summary>
    /// <param name="collection">The collection to break the relationship with.</param>
    public void RemoveChild(VltCollection collection)
    {
        if (!ReferenceEquals(collection.Parent, this))
        {
            throw new ArgumentException("Attempted to disassociate a non-related collection");
        }

        collection.Parent = null;
    }

    /// <summary>
    /// Changes the vault that the collection is associated with.
    /// </summary>
    /// <param name="vault">The new parent vault.</param>
    public void SetVault(Vault vault)
    {
        Vault = vault;
    }

    /// <summary>
    /// Gets a read-only copy of the collection's data dictionary.
    /// </summary>
    /// <remarks>This method does not perform any conversions. It returns the underlying objects for everything.</remarks>
    /// <returns>The read-only data dictionary.</returns>
    public IReadOnlyDictionary<string, object> GetData()
    {
        return Data.GetDictionary();
    }

    public IReadOnlyList<VltDataTable.Entry> GetOrderedData()
    {
        return Data.GetEntries();
    }

    /// <summary>
    /// Determines if the collection has a data entry with the given key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns><c>true</c> if an entry exists; otherwise, <c>false</c>.</returns>
    public bool HasEntry(string key) => Data.HasValue(key);

    /// <summary>
    /// Obtains the value mapped to <paramref name="key"/> from the collection's data dictionary.
    /// </summary>
    /// <param name="key">The name of the field to obtain the value of.</param>
    /// <returns>The <see cref="VltBaseType"/> instance mapped to <paramref name="key"/>.</returns>
    /// <exception cref="KeyNotFoundException">If there is no value mapped to <paramref name="key"/>.</exception>
    public object GetRawValue(string key)
    {
        return GetRawValue<object>(key);
    }

    /// <summary>
    /// Obtains the value mapped to <paramref name="key"/> from the collection's data dictionary.
    /// </summary>
    /// <param name="key">The name of the field to obtain the value of.</param>
    /// <returns>The <see cref="VltBaseType"/> instance mapped to <paramref name="key"/>.</returns>
    /// <exception cref="KeyNotFoundException">If there is no value mapped to <paramref name="key"/>.</exception>
    public T GetRawValue<T>(string key)
    {
        if (!Data.TryGetValue(key, out T data))
            throw new KeyNotFoundException($"Collection {ShortPath} does not have a value for field {key}");
        return data;
    }

    /// <summary>
    /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="key"/> in the collection's data dictionary.
    /// </summary>
    /// <typeparam name="T">The data type to be obtained.</typeparam>
    /// <param name="key">The mapping key.</param>
    /// <param name="index">The array index to retrieve the value from.</param>
    /// <returns>The mapping value.</returns>
    public T GetRawValue<T>(string key, int index) where T : VltBaseType
    {
        var data = GetRawValue(key, index);

        if (data is not T value)
            throw new InvalidCastException($"Field {key} is not compatible with type {typeof(T)}");

        return value;
    }

    public object GetRawValue(string key, int index)
    {
        VltArrayType array = GetRawValue<VltArrayType>(key);

        if (index < 0 || index >= array.Items.Count)
        {
            throw new ArgumentException($"Failed condition: 0 <= {index} < {array.Items.Count}");
        }

        return array.Items[index];
    }

    // public T GetDataValue<T>(string key, int index)
    // {
    //     return (T)BaseTypeToData(GetRawValue(key, index));
    // }

    /// <summary>
    /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    /// </summary>
    /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    /// <param name="data">The mapping value.</param>
    public void SetRawValue(string key, object data)
    {
        if (Class.HasField(key))
        {
            Data.SetValue(key, data);
        }
        else
        {
            throw new KeyNotFoundException($"Class '{Class.Name}' does not have field '{key}'");
        }
    }

    // /// <summary>
    // /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    // /// </summary>
    // /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    // /// <param name="data">The mapping value.</param>
    // public void SetDataValue<T>(string key, T data)
    // {
    //     if (Class.HasField(key))
    //     {
    //         if (HasEntry(key))
    //         {
    //             SetRawValue(key, DataToBaseType(Class[key], GetRawValue(key), data));
    //         }
    //         else
    //         {
    //             var rawValue =
    //                 Vault.Database.TypeRegistry.CreateInstance(Class, Class[key], this);
    //             SetRawValue(key, DataToBaseType(Class[key], rawValue, data));
    //         }
    //     }
    //     else
    //     {
    //         throw new KeyNotFoundException($"Class '{Class.Name}' does not have field '{key}'");
    //     }
    // }

    /// <summary>
    /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    /// </summary>
    /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    /// <param name="index"></param>
    /// <param name="data">The mapping value.</param>
    public void SetRawValue<T>(string key, int index, T data) where T : VltBaseType
    {
        VltArrayType array = GetRawValue<VltArrayType>(key);

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

    // /// <summary>
    // /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
    // /// </summary>
    // /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
    // /// <param name="index"></param>
    // /// <param name="data">The mapping value.</param>
    // public void SetDataValue<T>(string key, int index, T data)
    // {
    //     if (Class.HasField(key))
    //     {
    //         if (HasEntry(key))
    //         {
    //             SetRawValue(key, index, DataToBaseType(Class[key], GetRawValue(key, index), data));
    //         }
    //         else
    //         {
    //             var databaseTypeRegistry = Vault.Database.TypeRegistry;
    //             var rawValue =
    //                 databaseTypeRegistry.ConstructInstance(
    //                     databaseTypeRegistry.ResolveType(Class[key].TypeName), Class,
    //                     Class[key], this);
    //             SetRawValue(key, index, DataToBaseType(Class[key], rawValue, data));
    //         }
    //     }
    //     else
    //     {
    //         throw new KeyNotFoundException($"Class '{Class.Name}' does not have field '{key}'");
    //     }
    // }

    /// <summary>
    /// Removes an entry from the data dictionary.
    /// This is only valid for optional fields.
    /// </summary>
    /// <param name="key">The mapping key.</param>
    public void RemoveValue(string key)
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
                throw new KeyNotFoundException($"Collection '{ShortPath}' does not have an entry for '{key}'");
            }
        }
        else
        {
            throw new KeyNotFoundException($"Class '{Class.Name}' does not have field '{key}'");
        }
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