// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 5:45 PM.

using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Hashing;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.Attrib.Query
{
    [VltTypeInfo(
        "Attrib::Query::Static_Inorder_N_to_1<Attrib::Query::Typespace<Attrib::Key,Attrib::Key,EA::Reflection::UInt32> >")]
    public class Static_Inorder_N_to_1 : VltBaseType, IPointerObject
    {
        private long _leavesDst;
        private long _leavesPointer;
        private long _nodesDst;
        private long _nodesPointer;
        private uint _numRoots;

        private long _numRootsDst;
        private long _rootsDst;
        private long _rootsPointer;

        public Static_Inorder_N_to_1(VltClass @class, VltClassField field, VltCollection collection) : base(@class,
            field, collection)
        {
        }

        public Static_Inorder_N_to_1(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public void ReadPointerData(VaultReadContext context, BinaryReader br)
        {
            // This code only exists for testing
            // br.BaseStream.Position = _rootsPointer;
            //
            // // roots and nodes are sorted by key
            // var roots = new uint[_numRoots];
            //
            // for (var i = 0; i < _numRoots; i++)
            // {
            //     roots[i] = br.ReadUInt32();
            // }
            //
            // br.BaseStream.Position = _nodesPointer;
            //
            // var nodes = new (uint Index, uint NumChildren)[_numRoots];
            //
            // for (var i = 0; i < _numRoots; i++)
            // {
            //     nodes[i].Index = br.ReadUInt32();
            //     nodes[i].NumChildren = br.ReadUInt32();
            // }
            //
            // br.BaseStream.Position = _leavesPointer;
            //
            // // individual child lists are sorted by collection name?
            // // it probably doesn't actually matter but that's what it looks like
            // // the list of lists is sorted by key, i.e.,
            // // childLists[i] corresponds with nodes[i]
            // // corresponds with roots[i]
            // var childLists = new uint[_numRoots][];
            //
            // for (var i = 0; i < _numRoots; i++)
            // {
            //     childLists[i] = new uint[nodes[i].NumChildren];
            //
            //     for (var j = 0; j < childLists[i].Length; j++)
            //     {
            //         childLists[i][j] = br.ReadUInt32();
            //     }
            // }
            //
            // // Debugger.Break();
        }

        public void WritePointerData(VaultWriteContext context, BinaryWriter bw)
        {
            var groupedCollections = context.Database.RowManager.EnumerateFlattenedCollections(Class.Name)
                .GroupBy(c => Vlt32Hasher.Hash(c.Parent?.Name));
            var sortedGroups = groupedCollections.OrderBy(g => g.Key).ToList();

            var curPos = bw.BaseStream.Position;
            bw.BaseStream.Position = _numRootsDst;
            bw.Write(sortedGroups.Count);
            bw.BaseStream.Position = curPos;

            _rootsDst = bw.BaseStream.Position;
            foreach (var group in sortedGroups) bw.Write(group.Key);

            _nodesDst = bw.BaseStream.Position;
            var leafStartIndex = 0;
            foreach (var group in sortedGroups)
            {
                bw.Write(leafStartIndex);
                var numChildren = group.Count();
                bw.Write(numChildren);
                leafStartIndex += numChildren;
            }

            _leavesDst = bw.BaseStream.Position;
            foreach (var collection in sortedGroups.SelectMany(group => group.OrderBy(c => c.Name)))
                bw.Write(Vlt32Hasher.Hash(collection.Name));
        }

        public void AddPointers(VaultWriteContext context)
        {
            context.AddPointer(_rootsPointer, _rootsDst, false);
            context.AddPointer(_nodesPointer, _nodesDst, false);
            context.AddPointer(_leavesPointer, _leavesDst, false);
        }

        public override void Read(VaultReadContext context, BinaryReader br)
        {
            _numRoots = br.ReadUInt32();
            _rootsPointer = br.ReadPointer();
            _nodesPointer = br.ReadPointer();
            _leavesPointer = br.ReadPointer();
        }

        public override void Write(VaultWriteContext context, BinaryWriter bw)
        {
            _numRootsDst = bw.BaseStream.Position;
            bw.Write(0xAAAAAAAA);
            _rootsPointer = bw.WritePointer();
            _nodesPointer = bw.WritePointer();
            _leavesPointer = bw.WritePointer();
        }
    }
}