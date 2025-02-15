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
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types;

public class VltArrayType<TKey> : VltBaseType<TKey>, IReferencesStrings, IVltPointerObject<TKey>,
    IReferencesCollections<TKey>
    where TKey : struct, IKey<TKey>
{
    private VltClassField<TKey> _field;

    public VltArrayType(VltClassField<TKey> field, Type itemType)
    {
        _field = field;
        ItemAlignment = field.Alignment;
        ItemType = itemType;
        Items = new List<object>();
    }

    public ushort Capacity { get; set; }

    private int ItemAlignment { get; }

    public Type ItemType { get; }

    public IList<object> Items { get; set; }

    public IEnumerable<CollectionReferenceInfo<TKey>> GetReferencedCollections(Database<TKey> database,
        Vault<TKey> vault)
    {
        return Items.OfType<IReferencesCollections<TKey>>()
            .SelectMany(rc => rc.GetReferencedCollections(database, vault));
    }

    public bool ReferencesCollection(TKey classKey, TKey collectionKey)
    {
        return Items.OfType<IReferencesCollections<TKey>>().Any(rc => rc.ReferencesCollection(classKey, collectionKey));
    }

    /**
     * The reason these functions are implemented is because arrays may contain items that have pointers.
     * This system is complicated.
     */
    public IEnumerable<string> GetStrings()
    {
        foreach (var value in Items)
        {
            switch (value)
            {
                case string stringValue:
                    yield return stringValue;
                    break;
                case IReferencesStrings referencesStrings:
                {
                    foreach (var s in referencesStrings.GetStrings())
                    {
                        yield return s;
                    }

                    break;
                }
            }
        }
        // return Items.OfType<string>().Concat(Items.OfType<IReferencesStrings>().SelectMany(r => r.GetStrings()));
    }

    public void ReadPointerData(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryReader br)
    {
        foreach (var pointerObject in Items.OfType<IVltPointerObject<TKey>>())
            pointerObject.ReadPointerData(context, fieldContext, br);
    }

    public void WritePointerData(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        foreach (var pointerObject in Items.OfType<IVltPointerObject<TKey>>())
        {
            bw.AlignWriter(ItemAlignment);
            pointerObject.WritePointerData(context, fieldContext, bw);
        }
    }

    public void AddPointers(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext)
    {
        foreach (var pointerObject in Items.OfType<IVltPointerObject<TKey>>())
            pointerObject.AddPointers(context, fieldContext);
    }

    public override void Read(VaultReadContext<TKey> context, FieldReadWriteContext<TKey> fieldContext, BinaryReader br)
    {
        Capacity = br.ReadUInt16();
        var count = br.ReadUInt16();
        Debug.Assert(count <= Capacity, "count <= Capacity",
            $"Array length ({count}) exceeds capacity ({Capacity})");
        Items = new List<object>();
        var fieldSize = br.ReadUInt16();

        Debug.Assert(fieldSize == fieldContext.Field.Size, "fieldSize == fieldContext.Field.Size",
            $"Array item size is {fieldSize}, but it should be {fieldContext.Field.Size}");

        var encodedTypePad = br.ReadUInt16();
        var pad = (encodedTypePad >> 12) & 8;

        br.BaseStream.Position += pad;

        var databaseTypeRegistry = context.Database.TypeRegistry;

        for (var i = 0; i < count; i++)
        {
            var start = br.BaseStream.Position;
            var fieldAlignment = fieldContext.Field.Alignment;
            Debug.Assert(start % fieldAlignment == 0, "start % fieldAlignment == 0",
                $"Array reader @ 0x{start:X} (item {i}) is not 0x{fieldAlignment:X}-aligned");
            var item = databaseTypeRegistry.ReadTypeInstance(context, fieldContext, br);
            var end = br.BaseStream.Position;
            var bytesRead = end - start;
            Debug.Assert(bytesRead == fieldSize, "bytesRead == fieldSize",
                $"Read {bytesRead} bytes for array item of type {ItemType} instead of expected {fieldSize}");
            Items.Add(item);
        }

        br.BaseStream.Position += (Capacity - count) * fieldSize;
    }

    public override void Write(VaultWriteContext<TKey> context, FieldReadWriteContext<TKey> fieldContext,
        BinaryWriter bw)
    {
        bw.Write(Capacity);
        bw.Write((ushort)Items.Count);
        var fieldSize = fieldContext.Field.Size;
        bw.Write((ushort)fieldSize);

        var dataStartPos = bw.BaseStream.Position + sizeof(ushort);
        var alignedDataStartPos = (dataStartPos + (ItemAlignment - 1)) & ~(ItemAlignment - 1);
        Debug.Assert(alignedDataStartPos >= dataStartPos, "alignedDataStartPos >= dataStartPos");
        var alignmentOffset = alignedDataStartPos - dataStartPos;
        // Debug.Assert(alignmentOffset % 8 == 0, "alignmentOffset % 8 == 0");
        Debug.Assert(alignmentOffset <= 8, "alignmentOffset <= 8");
        bw.Write((ushort)(alignmentOffset << 12));

        bw.BaseStream.Position += alignmentOffset;

        var fieldAlignment = fieldContext.Field.Alignment;
        for (var i = 0; i < Items.Count; i++)
        {
            var t = Items[i];
            var start = bw.BaseStream.Position;

            Debug.Assert(start % fieldAlignment == 0, "start % fieldAlignment == 0",
                $"Array writer @ 0x{start:X} (item {i}) is not 0x{fieldAlignment:X}-aligned");
            context.Database.TypeRegistry.WriteTypeInstance(fieldContext.Field, t, context, fieldContext, bw);
            var end = bw.BaseStream.Position;
            var bytesWritten = end - start;

            Debug.Assert(bytesWritten == fieldSize, "bytesWritten == fieldSize",
                $"Wrote {bytesWritten} bytes for array item of type {ItemType} instead of expected {fieldSize}");
        }

        for (var i = 0; i < Capacity - Items.Count; i++)
        {
            var start = bw.BaseStream.Position;
            Debug.Assert(start % fieldAlignment == 0, "start % Field.Alignment == 0");
            bw.Write(new byte[fieldSize]);
        }
    }

    public override string ToString()
    {
        return string.Join(" | ", Items);
    }

    public override object Clone()
    {
        if (typeof(IComplexType).IsAssignableFrom(this.ItemType))
        {
            return new VltArrayType<TKey>(_field, ItemType)
            {
                Capacity = Capacity,
                Items = this.Items.Cast<IComplexType>().Select(c => c.Clone()).ToList(),
            };
        }

        return new VltArrayType<TKey>(_field, ItemType)
        {
            Capacity = Capacity,
            Items = new List<object>(this.Items)
        };
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