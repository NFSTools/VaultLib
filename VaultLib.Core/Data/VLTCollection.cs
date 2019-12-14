// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:04 PM.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VaultLib.Core.Types;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Data
{
    /// <summary>
    ///     A collection in VLT is like a row in a SQL database.
    ///     A collection specifies values for the fields of its class.
    /// </summary>
    public class VltCollection : IEquatable<VltCollection>
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
        /// Gets the child collections of this collection.
        /// </summary>
        public ObservableCollection<VltCollection> Children { get; }

        /// <summary>
        /// Gets the collection's data.
        /// </summary>
        /// <remarks> This is a mapping between a <see cref="VltClassField"/>'s name and a <see cref="VLTBaseType"/> instance.</remarks>
        private Dictionary<string, VLTBaseType> Data { get; }

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
            Children = new ObservableCollection<VltCollection>();
            Data = new Dictionary<string, VLTBaseType>();
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
        /// Adds the given collection to the list of child collections and associates it with its new parent.
        /// </summary>
        /// <param name="collection">The collection to add to the child list.</param>
        public void AddChild(VltCollection collection)
        {
            if (Children.Contains(collection))
            {
                throw new ArgumentException("Attempted to add a collection as a child when it is already a child of this collection.");
            }

            Children.Add(collection);

            // Disassociate old parent
            collection.Parent?.RemoveChild(collection);
            // Set new parent
            collection.Parent = this;
        }

        /// <summary>
        /// Removes the given collection from the list of child collections and disassociates it from its parent collection.
        /// </summary>
        /// <param name="collection">The collection to remove from the child list.</param>
        public void RemoveChild(VltCollection collection)
        {
            if (!Children.Contains(collection))
            {
                throw new ArgumentException("Attempted to remove a collection from the list of children when it is not a child of this collection.");
            }

            Children.Remove(collection);
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
        /// <returns>The read-only dictionary.</returns>
        public IReadOnlyDictionary<string, VLTBaseType> GetData()
        {
            return new ReadOnlyDictionary<string, VLTBaseType>(Data);
        }

        /// <summary>
        /// Retrieves the <see cref="VLTBaseType"/> mapped to the given key in the collection's data dictionary.
        /// </summary>
        /// <param name="key">The key to search for in the data dictionary.</param>
        /// <param name="data">A <see cref="VLTBaseType"/> reference to be filled.</param>
        /// <returns><c>true</c> if the data dictionary has a mapping for the given key; otherwise, <c>false</c>.</returns>
        public bool GetDataValue(string key, out VLTBaseType data)
        {
            return Data.TryGetValue(key, out data);
        }

        /// <summary>
        /// Gets the <see cref="VLTBaseType"/> mapped to the given key in the collection's data dictionary.
        /// </summary>
        /// <param name="key">The key to search for in the data dictionary.</param>
        /// <returns>The <see cref="VLTBaseType"/> instance mapped to the given key.</returns>
        /// <exception cref="KeyNotFoundException">A mapping for <paramref name="key"/> does not exist.</exception>
        public VLTBaseType GetDataValue(string key)
        {
            return Data[key];
        }

        /// <summary>
        /// Gets the <see cref="VLTBaseType"/> mapped to the given key in the collection's data dictionary.
        /// </summary>
        /// <typeparam name="T">The specific data type to obtain.</typeparam>
        /// <param name="key">The key to search for in the data dictionary.</param>
        /// <returns>The <see cref="VLTBaseType"/> instance mapped to the given key.</returns>
        /// <exception cref="KeyNotFoundException">A mapping for <paramref name="key"/> does not exist.</exception>
        public T GetDataValue<T>(string key) where T : VLTBaseType
        {
            return GetValue<T>(GetDataValue(key));
        }

        /// <summary>
        /// Gets the <see cref="List{T}"/> representation of the <see cref="VLTArrayType"/> instance mapped to the given key.
        /// </summary>
        /// <typeparam name="T">The specific data type to obtain a list of.</typeparam>
        /// <param name="key">The key to search for in the data dictionary.</param>
        /// <returns>The <see cref="List{T}"/></returns>
        /// <exception cref="KeyNotFoundException">A mapping for <paramref name="key"/> does not exist.</exception>
        public List<T> GetDataList<T>(string key) where T : VLTBaseType
        {
            return GetArray<T>(GetDataValue<VLTArrayType>(key));
        }

        /// <summary>
        /// Gets the <see cref="VLTBaseType"/> at the given index in the array mapped to the given key in the collection's data dictionary.
        /// </summary>
        /// <typeparam name="T">The specific data type to obtain.</typeparam>
        /// <param name="key">The key to search for in the data dictionary.</param>
        /// <param name="index">The index to fetch from the array</param>
        /// <returns>The <see cref="VLTBaseType"/> instance at the given index.</returns>
        /// <exception cref="KeyNotFoundException">A mapping for <paramref name="key"/> does not exist.</exception>
        /// <exception cref="IndexOutOfRangeException">The given index is out of the allowed range.</exception>
        public T GetDataValue<T>(string key, int index) where T : VLTBaseType
        {
            VLTArrayType array = GetDataValue<VLTArrayType>(key);

            if (index >= array.Items.Length)
            {
                throw new IndexOutOfRangeException($"0 <= index <= {array.Items.Length}");
            }

            return GetValue<T>(array.Items[index]);
        }

        /// <summary>
        /// Updates or creates a mapping in the collection's data dictionary with the given key and value.
        /// </summary>
        /// <param name="key">The key for the mapping.</param>
        /// <param name="data">The <see cref="VLTBaseType"/> instance that is the mapping value.</param>
        public void SetDataValue(string key, VLTBaseType data)
        {
            Data[key] = data;
        }

        /// <summary>
        /// Removes the data mapping with the given key, if one exists.
        /// </summary>
        /// <param name="key">The key for the mapping to be removed.</param>
        /// <returns><c>true</c> if a mapping was removed; otherwise, <c>false</c>.</returns>
        public bool RemoveDataValue(string key)
        {
            return Data.Remove(key);
        }

        #endregion

        #region IEquatable Members

        public bool Equals(VltCollection other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Class, other.Class) && string.Equals(Name, other.Name, StringComparison.InvariantCulture) && Equals(Parent, other.Parent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((VltCollection)obj);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(Class);
            hashCode.Add(Name, StringComparer.InvariantCulture);
            hashCode.Add(Parent);
            return hashCode.ToHashCode();
        }

        #endregion

        #region Internal API Implementation

        private T GetValue<T>(VLTBaseType originalValue)
        {
            Type genericType = typeof(T);

            switch (true)
            {
                case true when genericType.IsPrimitive:
                    return (T)((PrimitiveTypeBase)originalValue).GetValue();
                case true when genericType == typeof(string):
                    return (T)Convert.ChangeType(((IStringValue)originalValue).GetString(), typeof(T));
                default:
                    return (T)(object)null;
            }
        }

        private List<T> GetArray<T>(VLTArrayType array)
        {
            VLTArrayType vltArray = array;
            List<T> list = new List<T>();

            foreach (var item in vltArray.Items)
            {
                list.Add(GetValue<T>(item));
            }

            return list;
        }

        #endregion
    }
}