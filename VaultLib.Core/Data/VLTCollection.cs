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
        /// Obtains the value mapped to <paramref name="key"/> from the collection's data dictionary.
        /// </summary>
        /// <param name="key">The name of the field to obtain the value of.</param>
        /// <returns>The <see cref="VLTBaseType"/> instance mapped to <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">If there is no value mapped to <paramref name="key"/>.</exception>
        public VLTBaseType GetDataValue(string key)
        {
            if (Data.TryGetValue(key, out VLTBaseType data))
            {
                return data;
            }

            throw new KeyNotFoundException($"Collection {ShortPath} does not have a value for field {key}");
        }

        /// <summary>
        /// Obtains the simplified representation of the value mapped to <paramref name="key"/> from the collection's data dictionary.
        /// </summary>
        /// <param name="key">The name of the field to obtain the value of.</param>
        /// <returns>The simplified representation of the <see cref="VLTBaseType"/> instance mapped to <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">If there is no value mapped to <paramref name="key"/>.</exception>
        public object GetSimpleValue(string key)
        {
            if (Data.TryGetValue(key, out VLTBaseType data))
            {
                return GetUnderlyingValue(data);
            }

            throw new KeyNotFoundException($"Collection {ShortPath} does not have a value for field {key}");
        }

        /// <summary>
        /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
        /// </summary>
        /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
        /// <param name="data">The mapping value.</param>
        public void SetDataValue(string key, VLTBaseType data)
        {
            Data[key] = data;
        }

        /// <summary>
        /// Updates or creates a mapping in the data dictionary between <paramref name="key"/> and <paramref name="data"/>.
        /// </summary>
        /// <param name="key">The mapping key. (Typically the VLT field name.)</param>
        /// <param name="data">The mapping value.</param>
        public void SetDataValue<T>(string key, T data)
        {
            SetDataValue(key,
                Data.TryGetValue(key, out VLTBaseType originalData)
                    ? ConvertPrimitiveToBaseType(data, originalData)
                    : ConvertPrimitiveToBaseType(data,
                        TypeRegistry.CreateInstance(Vault.Database.Options.GameId, Class, Class[key], this)));
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="key"/> in the collection's data dictionary.
        /// </summary>
        /// <typeparam name="T">The data type to be obtained.</typeparam>
        /// <param name="key">The mapping key.</param>
        /// <returns>The mapping value.</returns>
        public T GetDataValue<T>(string key)
        {
            VLTBaseType originalValue = GetDataValue(key);
            Type genericType = typeof(T);

            return ConvertBaseType<T>(genericType, originalValue);
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="T"/> mapped to <paramref name="key"/> in the collection's data dictionary.
        /// </summary>
        /// <typeparam name="T">The data type to be obtained.</typeparam>
        /// <param name="key">The mapping key.</param>
        /// <param name="index">The array index to retrieve the value from.</param>
        /// <returns>The mapping value.</returns>
        public T GetDataValue<T>(string key, int index)
        {
            VLTArrayType array = GetDataValue<VLTArrayType>(key);

            if (index < 0 || index >= array.Items.Count)
            {
                throw new ArgumentException($"Failed condition: 0 <= {index} < {array.Items.Count}");
            }

            return ConvertBaseType<T>(typeof(T), array.Items[index]);
        }

        /// <summary>
        /// Gets the length of the array mapped to <paramref name="key"/> in the collection's data dictionary.
        /// </summary>
        /// <param name="key">The mapping key.</param>
        /// <returns>The array length.</returns>
        public int GetListLength(string key)
        {
            VLTArrayType array = GetDataValue<VLTArrayType>(key);

            return array.Items.Count;
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

        #region Internal Members

        private T ConvertBaseType<T>(Type genericType, VLTBaseType originalValue)
        {
            if (genericType.IsPrimitive)
            {
                if (originalValue is PrimitiveTypeBase ptb)
                {
                    return (T)ptb.GetValue();
                }

                throw new Exception($"Cannot map {originalValue.GetType()} to {genericType}");
            }

            if (genericType == typeof(string))
            {
                // string isn't a primitive - cool...
                if (originalValue is IStringValue sv)
                {
                    return (T)(object)sv.GetString();
                }

                throw new Exception($"Cannot map {originalValue.GetType()} to {genericType}");
            }

            return (T)(object)originalValue;
        }

        private object GetUnderlyingValue(VLTBaseType originalValue)
        {
            switch (originalValue)
            {
                case PrimitiveTypeBase ptb:
                    return ptb.GetValue();
                case IStringValue sv:
                    return sv.GetString();
                case VLTArrayType array:
                    return array.Items;
                default:
                    return originalValue;
            }
        }

        private VLTBaseType ConvertPrimitiveToBaseType(object data, VLTBaseType baseType)
        {
            if (data is VLTBaseType vltBaseType) return vltBaseType;

            switch (baseType)
            {
                case VLTArrayType array:
                    {
                        if (data is IList<VLTBaseType> list)
                        {
                            array.Items = list;
                        }

                        break;
                    }
                case IStringValue sv when data is string s:
                    sv.SetString(s);
                    break;
                case IStringValue sv:
                    throw new Exception($"Cannot map {data.GetType()} to string");
                case PrimitiveTypeBase ptb when data is IConvertible ico:
                    ptb.SetValue(ico);
                    break;
                case PrimitiveTypeBase ptb:
                    throw new Exception($"Cannot map {data.GetType()} to IConvertible");
            }

            return baseType;
        }

        #endregion
    }
}