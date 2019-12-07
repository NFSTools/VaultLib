// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 7:04 PM.

using System;
using System.Collections.Generic;
using VaultLib.Core.External.CollectionViews;
using VaultLib.Core.Types;

namespace VaultLib.Core.Data
{
    /// <summary>
    ///     A collection in VLT is like a row in a SQL database.
    ///     A collection specifies values for the fields of its class.
    /// </summary>
    public class VLTCollection : IEquatable<VLTCollection>
    {
        public VLTCollection(Vault vault, VLTClass vltClass, string name, ulong key)
        {
            Vault = vault;
            Class = vltClass;
            ClassName = vltClass.Name;
            Name = name;
            Key = key;
            DataRow = new Dictionary<ulong, VLTBaseType>();
            Children = new FastObservableCollection<VLTCollection>();
        }

        /// <summary>
        ///     The name of the collection
        /// </summary>
        public string Name { get; private set; }

        public ulong Key { get; set; }
        public ulong ParentKey { get; set; }
        public uint Level { get; set; }

        public VLTClass Class { get; }

        public string ClassName { get; }

        /// <summary>
        ///     The collection's data
        /// </summary>
        public Dictionary<ulong, VLTBaseType> DataRow { get; }

        /// <summary>
        ///     The child collections of the collection
        /// </summary>
        public FastObservableCollection<VLTCollection> Children { get; }

        /// <summary>
        ///     A <see cref="CollectionView" /> instance that does some basic manipulation (sorting)
        ///     of the list of child collections. This is useful for actually displaying the data.
        /// </summary>
        /// <remarks>I can't believe I actually pulled in an entire CollectionView library. Alas, it was a necessity.</remarks>
        public CollectionView ChildrenView
        {
            get
            {
                var view = new CollectionView(Children);
                view.SortDescriptions.Add(new SortDescription("Name"));

                return view;
            }
        }

        public Vault Vault { get; }
        public VLTCollection Parent { get; private set; }

        public string ShortPath => $"{ClassName}/{Name}";

        public string FullPath
        {
            get
            {
                var path = "";

                var parent = Parent;

                while (parent != null)
                {
                    path = $"/{parent.Name}" + path;
                    parent = parent.Parent;
                }

                return $"{ClassName}{path}/{Name}";
            }
        }

        public bool Equals(VLTCollection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ClassName == other.ClassName && Name == other.Name;
        }

        /// <summary>
        ///     Makes the current collection a child of the given collection.
        /// </summary>
        /// <param name="collection"></param>
        public void SetParent(VLTCollection collection)
        {
            Parent = collection;
            Level = Parent.Level + 1;
            Parent.Children.Add(this);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((VLTCollection) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = ClassName.GetHashCode();

            if (Name != null)
                hashCode ^= 397 * Name.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(VLTCollection left, VLTCollection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VLTCollection left, VLTCollection right)
        {
            return !Equals(left, right);
        }
    }
}