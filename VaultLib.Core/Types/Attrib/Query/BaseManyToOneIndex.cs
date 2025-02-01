// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 5:45 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Query;

public abstract class BaseManyToOneIndex<TIndexKey, TIndexValue> : VltBaseType<Key32>, IVltPointerObject<Key32>
    where TIndexKey : unmanaged, IComparable<TIndexKey>
    where TIndexValue : unmanaged
{
    protected record IndexEntry(TIndexKey Key, List<TIndexValue> Values);

    private long _valsDst;
    private long _valsPointer;
    private long _indicesDst;
    private long _indicesPointer;
    private int _count;

    private long _countDst;
    private long _keysDst;
    private long _keysPointer;

    public override void Read(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        _count = br.ReadInt32();
        _keysPointer = br.ReadPointer();
        _indicesPointer = br.ReadPointer();
        _valsPointer = br.ReadPointer();

        Debug.WriteLine("Static_Inorder_N_to_1::Read - class={0} count={1}", fieldContext.Class.Key, _count);
    }

    public override void Write(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        _countDst = bw.BaseStream.Position;
        bw.Write(0xAAAAAAAA);
        _keysPointer = bw.WritePointer();
        _indicesPointer = bw.WritePointer();
        _valsPointer = bw.WritePointer();
    }

    public void ReadPointerData(VaultReadContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryReader br)
    {
        br.BaseStream.Position = _keysPointer;

        var keys = ReadKeys(context, fieldContext, br, _count);

        var sortedKeys = keys.OrderBy(x => x);
        Debug.Assert(keys.SequenceEqual(sortedKeys));

        br.BaseStream.Position = _indicesPointer;
        var indices = new List<(int Index, int Count)>();
        for (var i = 0; i < _count; i++)
        {
            var index = br.ReadInt32();
            var count = br.ReadInt32();
            indices.Add((index, count));
        }

        br.BaseStream.Position = _valsPointer;
        var values = new List<TIndexValue>();
        foreach (var (_, count) in indices)
        {
            var blockValues = ReadValues(context, fieldContext, br, count);

            values.AddRange(blockValues);
        }

        Debug.WriteLine("{0} - class {1} has {2} keys, {3} values", GetType().Name, fieldContext.Class.Key, keys.Count,
            values.Count);

        for (var i = 0; i < indices.Count; i++)
        {
            var (index, count) = indices[i];
            var subValues = values.GetRange(index, count);
            Debug.WriteLine("key {0} - values ({2}): {1}", keys[i], string.Join(", ", subValues), count);
        }
    }

    public void WritePointerData(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw)
    {
        var entries = GenerateIndex(context, fieldContext);
        var sortedEntries = entries.OrderBy(x => x.Key).ToList();

        var curPos = bw.BaseStream.Position;
        bw.BaseStream.Position = _countDst;
        bw.Write(entries.Count);
        bw.BaseStream.Position = curPos;

        _keysDst = bw.BaseStream.Position;

        var sortedKeys = sortedEntries.Select(e => e.Key);
        WriteKeys(context, fieldContext, bw, sortedKeys);

        _indicesDst = bw.BaseStream.Position;
        var valuesStartIndex = 0;
        foreach (var group in sortedEntries)
        {
            bw.Write(valuesStartIndex);
            var numValues = group.Values.Count;
            bw.Write(numValues);
            valuesStartIndex += numValues;
        }

        _valsDst = bw.BaseStream.Position;
        var values = sortedEntries.SelectMany(e => e.Values);
        WriteValues(context, fieldContext, bw, values);
    }

    public void AddPointers(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext)
    {
        context.AddPointer(_keysPointer, _keysDst, false);
        context.AddPointer(_indicesPointer, _indicesDst, false);
        context.AddPointer(_valsPointer, _valsDst, false);
    }

    protected abstract List<TIndexKey> ReadKeys(VaultReadContext<Key32> context,
        FieldReadWriteContext<Key32> fieldContext, BinaryReader br, int count);

    protected abstract List<TIndexValue> ReadValues(VaultReadContext<Key32> context,
        FieldReadWriteContext<Key32> fieldContext, BinaryReader br, int count);

    protected abstract List<IndexEntry> GenerateIndex(VaultWriteContext<Key32> context,
        FieldReadWriteContext<Key32> fieldContext);

    protected abstract void WriteKeys(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw, IEnumerable<TIndexKey> keys);

    protected abstract void WriteValues(VaultWriteContext<Key32> context, FieldReadWriteContext<Key32> fieldContext,
        BinaryWriter bw, IEnumerable<TIndexValue> values);
}