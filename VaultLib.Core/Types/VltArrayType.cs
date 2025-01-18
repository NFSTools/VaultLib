// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 8:20 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VltArrayType : VltBaseType, IReferencesStrings, IReferencesCollections
    {
        public VltArrayType(VltClass @class, VltClassField field, VltCollection collection, Type itemType) : base(
            @class, field,
            collection)
        {
            ItemType = itemType;
            Items = new List<object>();
        }

        public VltArrayType(VltClass @class, VltClassField field, Type itemType) : this(@class, field, null, itemType)
        {
        }

        public ushort FieldSize { get; set; }

        public ushort Capacity { get; set; }

        public int ItemAlignment { get; set; }

        public Type ItemType { get; }

        public IList<object> Items { get; set; }

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

        public void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            foreach (var pointerObject in Items.OfType<IPointerObject>()) pointerObject.ReadPointerData(context, br);
        }

        public void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            foreach (var pointerObject in Items.OfType<IPointerObject>())
            {
                bw.AlignWriter(ItemAlignment);
                pointerObject.WritePointerData(context, bw);
            }
        }

        public void AddPointers(VaultWriteContext context)
        {
            foreach (var pointerObject in Items.OfType<IPointerObject>()) pointerObject.AddPointers(context);
        }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            Capacity = br.ReadUInt16();
            var count = br.ReadUInt16();
            Debug.Assert(count <= Capacity);
            Items = new List<object>();
            FieldSize = br.ReadUInt16();

            var encodedTypePad = br.ReadUInt16();
            var pad = (encodedTypePad >> 12) & 8;

            br.BaseStream.Position += pad;

            var databaseTypeRegistry = context.Database.TypeRegistry;

            for (var i = 0; i < count; i++)
            {
                var start = br.BaseStream.Position;
                Debug.Assert(start % Field.Alignment == 0, "start % Field.Alignment == 0");
                var item = databaseTypeRegistry.ReadTypeInstance(Class, Field, Collection, context, br);
                var end = br.BaseStream.Position;
                Debug.Assert(end - start == FieldSize, "end - start == FieldSize");
                Items.Add(item);
            }

            br.BaseStream.Position += (Capacity - count) * FieldSize;
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            bw.Write(Capacity);
            bw.Write((ushort)Items.Count);
            bw.Write(FieldSize);

            var dataStartPos = bw.BaseStream.Position + sizeof(ushort);
            var alignedDataStartPos = (dataStartPos + (ItemAlignment - 1)) & ~(ItemAlignment - 1);
            Debug.Assert(alignedDataStartPos >= dataStartPos, "alignedDataStartPos >= dataStartPos");
            var alignmentOffset = alignedDataStartPos - dataStartPos;
            Debug.Assert(alignmentOffset % 8 == 0, "alignmentOffset % 8 == 0");
            Debug.Assert(alignmentOffset <= 8, "alignmentOffset <= 8");
            bw.Write((ushort)(alignmentOffset << 12));

            bw.BaseStream.Position += alignmentOffset;

            foreach (var t in Items)
            {
                var start = bw.BaseStream.Position;
                Debug.Assert(start % Field.Alignment == 0, "start % Field.Alignment == 0");
                context.Database.TypeRegistry.WriteTypeInstance(Field, t, context, bw);
                var end = bw.BaseStream.Position;
                Debug.Assert(end - start == FieldSize, "end - start == FieldSize");
            }

            for (var i = 0; i < Capacity - Items.Count; i++)
            {
                var start = bw.BaseStream.Position;
                Debug.Assert(start % Field.Alignment == 0, "start % Field.Alignment == 0");
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

            return (T)Items[index];
        }

        /// <summary>
        /// Changes the value stored at the given index in the array
        /// </summary>
        /// <param name="index">The item index</param>
        /// <param name="value">The new item</param>
        public void SetValue(int index, object value)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new IndexOutOfRangeException($"Index must be in range [0, {Items.Count})");
            }

            Items[index] = value;
        }

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
        //         VltArrayType _ => throw new ApplicationException("Having an array of arrays is not possible..."),
        //         _ => baseType
        //     };
        // }

    //     private VltBaseType DataToBaseType(VltClassField field, VltBaseType originalData, object data)
    //     {
    //         switch (data)
    //         {
    //             case string s:
    //             {
    //                 if (originalData is IStringValue sv)
    //                 {
    //                     sv.SetString(s);
    //                     return originalData;
    //                 }
    //
    //                 break;
    //             }
    //             case IConvertible ic:
    //             {
    //                 if (originalData is PrimitiveTypeBase ptb)
    //                 {
    //                     ptb.SetValue(ic);
    //                     return originalData;
    //                 }
    //
    //                 break;
    //             }
    //             case VltBaseType vbt:
    //                 if (vbt is VltArrayType)
    //                     throw new ApplicationException("Array DataToBaseType cannot accept a VLTArrayType instance!");
    //                 return vbt;
    //         }
    //
    //         throw new ArgumentException($"Cannot convert {data.GetType()} to VLTBaseType.");
    //     }
    //
    #endregion
    }
}