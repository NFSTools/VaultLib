// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:20 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VLTArrayType : VLTBaseType, IReferencesStrings, IReferencesCollections
    {
        public VLTArrayType(VltClass @class, VltClassField field, VltCollection collection, Type itemType) : base(@class, field,
            collection)
        {
            ItemType = itemType;
            Items = new List<VLTBaseType>();
        }

        public VLTArrayType(VltClass @class, VltClassField field, Type itemType) : this(@class, field, null, itemType)
        {
        }

        public ushort FieldSize { get; set; }

        public ushort Capacity { get; set; }

        public int ItemAlignment { get; set; }

        public Type ItemType { get; }

        public IList<VLTBaseType> Items { get; set; }

        public IEnumerable<CollectionReferenceInfo> GetReferencedCollections(Database database, Vault vault)
        {
            return Items.OfType<IReferencesCollections>()
                .SelectMany(rc => rc.GetReferencedCollections(database, vault));
        }

        public bool ReferencesCollection(string classKey, string collectionKey)
        {
            return Items.OfType<IReferencesCollections>().Any(rc => rc.ReferencesCollection(classKey, collectionKey));
        }

        /**
         * The reason these functions are implemented is because arrays may contain items that have pointers.
         * This system is complicated.
         */

        public IEnumerable<string> GetStrings()
        {
            return Items.OfType<IReferencesStrings>().SelectMany(r => r.GetStrings());
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            foreach (var pointerObject in Items.OfType<IPointerObject>()) pointerObject.ReadPointerData(vault, br);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            foreach (var pointerObject in Items.OfType<IPointerObject>())
            {
                bw.AlignWriter(ItemAlignment);
                pointerObject.WritePointerData(vault, bw);
            }
        }

        public void AddPointers(Vault vault)
        {
            foreach (var pointerObject in Items.OfType<IPointerObject>()) pointerObject.AddPointers(vault);
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            Capacity = br.ReadUInt16();
            var count = br.ReadUInt16();
            Debug.Assert(count <= Capacity);
            Items = new List<VLTBaseType>();
            FieldSize = br.ReadUInt16();

            // NOTE: this is 0x8000 when Attrib::Types::Vector4 is in use. not sure why. 0 otherwise
            br.ReadUInt16();

            for (var i = 0; i < count; i++)
            {
                var item = TypeRegistry.ConstructInstance(ItemType, Class, Field, Collection);

                br.AlignReader(ItemAlignment);

                var start = br.BaseStream.Position;
                item.Read(vault, br);
                Debug.Assert(br.BaseStream.Position - start == FieldSize);
                Items.Add(item);
            }

            br.BaseStream.Position += (Capacity - count) * FieldSize;
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Capacity);
            bw.Write((ushort)Items.Count);
            bw.Write(FieldSize);
            bw.Write((ushort)(1 << (Field.Alignment - 1)));

            foreach (var t in Items)
            {
                bw.AlignWriter(ItemAlignment);
                var start = bw.BaseStream.Position;
                t.Write(vault, bw);
                Debug.Assert(bw.BaseStream.Position - start == FieldSize);
            }

            for (var i = 0; i < Capacity - Items.Count; i++)
            {
                bw.AlignWriter(ItemAlignment);
                bw.Write(new byte[FieldSize]);
            }
        }

        public override string ToString()
        {
            return string.Join(" | ", Items);
        }

        /// <summary>
        /// Gets the value stored at the given index in the array
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="index">The item index</param>
        /// <returns>The value stored at the given index</returns>
        public T GetValue<T>(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new IndexOutOfRangeException($"Index must be in range [0, {Items.Count})");
            }

            return (T) BaseTypeToData(Items[index]);
        }

        /// <summary>
        /// Changes the value stored at the given index in the array
        /// </summary>
        /// <typeparam name="T">The value type</typeparam>
        /// <param name="index">The item index</param>
        /// <param name="value">The new item</param>
        public void SetValue<T>(int index, T value)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new IndexOutOfRangeException($"Index must be in range [0, {Items.Count})");
            }

            Items[index] = DataToBaseType(Field, Items[index], value);
        }

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
                VLTArrayType _ => throw new ApplicationException("Having an array of arrays is not possible..."),
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
                    if (vbt is VLTArrayType)
                        throw new ApplicationException("Array DataToBaseType cannot accept a VLTArrayType instance!");
                    return vbt;
            }

            throw new ArgumentException($"Cannot convert {data.GetType()} to VLTBaseType.");
        }

        #endregion
    }
}