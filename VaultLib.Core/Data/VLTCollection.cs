// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:04 PM.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// Gets the short path of the collection.
        /// </summary>
        /// <example>gameplay/baseelement</example>
        public string ShortPath => $"{Class.Name}/{Name}";

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
        /// <remarks>This method does not perform any conversions. It returns the underlying objects for everything.</remarks>
        /// <returns>The read-only data dictionary.</returns>
        public IReadOnlyDictionary<string, VLTBaseType> GetData()
        {
            return new ReadOnlyDictionary<string, VLTBaseType>(Data);
        }

        /// <summary>
        /// Determines if the collection has a data entry with the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns><c>true</c> if an entry exists; otherwise, <c>false</c>.</returns>
        public bool HasEntry(string key) => Data.ContainsKey(key);

        /// <summary>
        /// Obtains the value mapped to <paramref name="key"/> from the collection's data dictionary.
        /// </summary>
        /// <param name="key">The name of the field to obtain the value of.</param>
        /// <returns>The <see cref="VLTBaseType"/> instance mapped to <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">If there is no value mapped to <paramref name="key"/>.</exception>
        public VLTBaseType GetRawValue(string key)
        {
            return GetRawValue<VLTBaseType>(key);
        }

        /// <summary>
        /// Obtains the value mapped to <paramref name="key"/> from the collection's data dictionary.
        /// </summary>
        /// <param name="key">The name of the field to obtain the value of.</param>
        /// <returns>The <see cref="VLTBaseType"/> instance mapped to <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">If there is no value mapped to <paramref name="key"/>.</exception>
        public T GetRawValue<T>(string key) where T : VLTBaseType
        {
            if (Data.TryGetValue(key, out VLTBaseType data))
            {
                return (T)data;
            }

            throw new KeyNotFoundException($"Collection {ShortPath} does not have a value for field {key}");
        }

        public T GetDataValue<T>(string key)
        {
            VLTBaseType originalData = GetRawValue(key);

            return (T)BaseTypeToData(originalData);
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="key"/> in the collection's data dictionary.
        /// </summary>
        /// <typeparam name="T">The data type to be obtained.</typeparam>
        /// <param name="key">The mapping key.</param>
        /// <param name="index">The array index to retrieve the value from.</param>
        /// <returns>The mapping value.</returns>
        public VLTBaseType GetRawValue(string key, int index)
        {
            return GetRawValue<VLTBaseType>(key, index);
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="key"/> in the collection's data dictionary.
        /// </summary>
        /// <typeparam name="T">The data type to be obtained.</typeparam>
        /// <param name="key">The mapping key.</param>
        /// <param name="index">The array index to retrieve the value from.</param>
        /// <returns>The mapping value.</returns>
        public T GetRawValue<T>(string key, int index) where T : VLTBaseType
        {
            VLTArrayType array = GetRawValue<VLTArrayType>(key);

            if (index < 0 || index >= array.Items.Count)
            {
                throw new ArgumentException($"Failed condition: 0 <= {index} < {array.Items.Count}");
            }

            return (T)array.Items[index];
        }

        public T GetDataValue<T>(string key, int index)
        {
            return (T) BaseTypeToData(GetRawValue(key, index));
        }

        /// <summary>
        /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
        /// </summary>
        /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
        /// <param name="data">The mapping value.</param>
        public void SetRawValue(string key, VLTBaseType data)
        {
            if (Class.HasField(key))
            {
                Data[key] = data;
            }
            else
            {
                throw new KeyNotFoundException($"Class '{Class.Name}' does not have field '{key}'");
            }
        }

        /// <summary>
        /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
        /// </summary>
        /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
        /// <param name="data">The mapping value.</param>
        public void SetDataValue<T>(string key, T data)
        {
            if (Class.HasField(key))
            {
                if (HasEntry(key))
                {
                    SetRawValue(key, DataToBaseType(Class[key], GetRawValue(key), data));
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

        /// <summary>
        /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
        /// </summary>
        /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
        /// <param name="index"></param>
        /// <param name="data">The mapping value.</param>
        public void SetRawValue<T>(string key, int index, T data) where T : VLTBaseType
        {
            VLTArrayType array = GetRawValue<VLTArrayType>(key);

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
        public void SetDataValue<T>(string key, int index, T data)
        {
            if (Class.HasField(key))
            {
                if (HasEntry(key))
                {
                    SetRawValue(key, index, DataToBaseType(Class[key], GetRawValue(key, index), data));
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

        #region IEquatable Members

        public bool Equals(VltCollection other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Class, other.Class) && Equals(Vault, other.Vault) && string.Equals(Name, other.Name, StringComparison.InvariantCulture) && Equals(Parent, other.Parent) && Equals(Children, other.Children) && Equals(Data, other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(VltCollection) && Equals((VltCollection)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Class);
            hashCode.Add(Vault);
            hashCode.Add(Name, StringComparer.InvariantCulture);
            hashCode.Add(Parent);
            hashCode.Add(Children);
            hashCode.Add(Data);
            return hashCode.ToHashCode();
        }

        #endregion

        #region Internal stuff

        private object BaseTypeToData(VLTBaseType baseType)
        {
            // if we have a primitive or string value, return that
            // if we have an array, return a list where each item in the array has been converted (recursion FTW)
            // otherwise, just return the original data

            return baseType switch
            {
                PrimitiveTypeBase ptb => ptb.GetValue(),
                IStringValue sv => sv.GetString(),
                VLTArrayType array => array.Items.Select(BaseTypeToData).ToList(),
                _ => baseType
            };
        }

        private VLTBaseType DataToBaseType(VltClassField field, VLTBaseType originalData, object data)
        {
            switch (data)
            {
                case string s:
                    {
                        if (originalData is IStringValue sv)
                        {
                            sv.SetString(s);
                            return originalData;
                        }

                        break;
                    }
                case IConvertible ic:
                    {
                        if (originalData is PrimitiveTypeBase ptb)
                        {
                            ptb.SetValue(ic);
                            return originalData;
                        }
                        break;
                    }
                case VLTBaseType vbt:
                    return vbt;
            }

            throw new ArgumentException($"Cannot convert {data.GetType()} to VLTBaseType.");
        }

        #endregion
    }
}