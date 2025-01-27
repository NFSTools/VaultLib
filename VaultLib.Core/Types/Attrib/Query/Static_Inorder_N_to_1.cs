// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 5:45 PM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.Hashing;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib.Query;

public class Static_Inorder_N_to_1 : VltBaseType<uint>, IVltPointerObject<uint>
{
    public enum TreeNodeType
    {
        ChildKeys,
        ParentKey
    }

    private long _valsDst;
    private long _valsPointer;
    private long _indicesDst;
    private long _indicesPointer;
    private uint _count;

    private long _countDst;
    private long _keysDst;
    private long _keysPointer;

    /// <summary>
    /// Gets or sets a value indicating the type of data stored in the tree.
    /// </summary>
    public TreeNodeType NodeType { get; set; }

    internal List<uint> Keys { get; private set; }
    internal List<uint> Values { get; private set; }
    internal List<(int Index, int Count)> Indices { get; private set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        _count = br.ReadUInt32();
        _keysPointer = br.ReadPointer();
        _indicesPointer = br.ReadPointer();
        _valsPointer = br.ReadPointer();

        Debug.WriteLine("Static_Inorder_N_to_1::Read - class={0} count={1}", fieldContext.Class.Name, _count);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        _countDst = bw.BaseStream.Position;
        bw.Write(0xAAAAAAAA);
        _keysPointer = bw.WritePointer();
        _indicesPointer = bw.WritePointer();
        _valsPointer = bw.WritePointer();
    }

    public void ReadPointerData(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryReader br)
    {
        br.BaseStream.Position = _keysPointer;
        Keys = new List<uint>();
        for (var i = 0; i < _count; i++)
        {
            Keys.Add(br.ReadUInt32());
        }

        var sortedKeys = Keys.OrderBy(x => x);
        Debug.Assert(Keys.SequenceEqual(sortedKeys));

        br.BaseStream.Position = _indicesPointer;
        Indices = new List<(int Index, int Count)>();
        for (var i = 0; i < _count; i++)
        {
            var index = br.ReadInt32();
            var count = br.ReadInt32();
            Indices.Add((index, count));
        }

        br.BaseStream.Position = _valsPointer;
        Values = new List<uint>();
        foreach (var (_, count) in Indices)
        {
            var blockValues = new List<uint>();
            for (var i = 0; i < count; i++)
            {
                blockValues.Add(br.ReadUInt32());
            }

            Values.AddRange(blockValues);
        }
    }

    public void WritePointerData(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext,
        BinaryWriter bw)
    {
        var entries = NodeType switch
        {
            TreeNodeType.ChildKeys => GetChildKeyEntries(context, fieldContext),
            TreeNodeType.ParentKey => GetParentKeyEntries(context, fieldContext),
            _ => throw new Exception("Unknown TreeNodeType")
        };

        var sortedEntries = entries.OrderBy(x => x.Key).ToList();

        var curPos = bw.BaseStream.Position;
        bw.BaseStream.Position = _countDst;
        bw.Write(entries.Count);
        bw.BaseStream.Position = curPos;

        _keysDst = bw.BaseStream.Position;
        foreach (var group in sortedEntries)
        {
            bw.Write(group.Key);
        }

        _indicesDst = bw.BaseStream.Position;
        var leafStartIndex = 0;
        foreach (var group in sortedEntries)
        {
            bw.Write(leafStartIndex);
            var numChildren = group.Values.Count;
            bw.Write(numChildren);
            leafStartIndex += numChildren;
        }

        _valsDst = bw.BaseStream.Position;
        foreach (var value in sortedEntries.SelectMany(e => e.Values))
        {
            bw.Write(value);
        }
    }

    private static List<(uint Key, List<uint> Values)> GetParentKeyEntries(VaultWriteContext<uint> context,
        FieldReadWriteContext<uint> fieldContext)
    {
        return context.Database.RowManager.EnumerateCollections(fieldContext.Class.Name)
            .Select(c => (Vlt32Hasher.Hash(c.Name), Vlt32Hasher.Hash(c.Parent?.Name)))
            .Select(c => (c.Item1, new List<uint> { c.Item2 }))
            .ToList();
    }

    private static List<(uint Key, List<uint> Values)> GetChildKeyEntries(VaultWriteContext<uint> context,
        FieldReadWriteContext<uint> fieldContext)
    {
        var collectionsGroupedByParent = context.Database.RowManager.EnumerateCollections(fieldContext.Class.Name)
            .GroupBy(c => Vlt32Hasher.Hash(c.Parent?.Name));

        return collectionsGroupedByParent
            .Select(g => (g.Key, g.OrderBy(c => c.Name).Select(c => Vlt32Hasher.Hash(c.Name)).ToList()))
            .ToList();
    }

    public void AddPointers(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext)
    {
        context.AddPointer(_keysPointer, _keysDst, false);
        context.AddPointer(_indicesPointer, _indicesDst, false);
        context.AddPointer(_valsPointer, _valsDst, false);
    }
}