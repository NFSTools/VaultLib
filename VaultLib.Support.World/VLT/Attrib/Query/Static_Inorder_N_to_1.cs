// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/27/2019 @ 5:45 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;

namespace VaultLib.Support.World.VLT.Attrib.Query
{
    [VLTTypeInfo(
        "Attrib::Query::Static_Inorder_N_to_1<Attrib::Query::Typespace<Attrib::Key,Attrib::Key,EA::Reflection::UInt32> >")]
    public class Static_Inorder_N_to_1 : VLTBaseType, IPointerObject
    {
        public long ElementsPointer { get; private set; }
        public long ElementsDest { get; private set; }
        public long TreePointer { get; private set; }
        public long TreeDest { get; private set; }
        public long EndPointer { get; private set; }
        public long EndDest { get; private set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            uint numElements = br.ReadUInt32();

            List<uint> elements = new List<uint>();
            uint elementsPointer = br.ReadUInt32();
            uint treePointer = br.ReadUInt32();
            uint endPointer = br.ReadUInt32();

            for (int i = 0; i < numElements; i++)
            {
                if (br.BaseStream.Position < elementsPointer || br.BaseStream.Position > treePointer)
                {
                    throw new Exception("Static_Inorder read failed");
                }

                elements.Add(br.ReadUInt32());

                //Debug.WriteLine("\tArray element: {0:X} ({1})", elements[i], HashManager.ResolveVLT(elements[i]));
            }

            uint lastIndex = 0u;

            for (int i = 0; i < numElements; i++)
            {
                uint index = br.ReadUInt32();
                uint childrenCount = br.ReadUInt32();

                Debug.Assert(index == lastIndex);
                lastIndex += childrenCount;
                //Debug.WriteLine("\tTree element: I={0} C={1}", index, childrenCount);
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ElementsPointer = 0;
            TreePointer = 0;
            EndPointer = 0;

            // Obtain the full list of collections
            List<VLTCollection> allCollections = vault.SaveContext.Collections.Where(c => c.Class.Name == Class.Name).ToList();

            // Group list by parent
            Dictionary<ulong, List<VLTCollection>> groupedByParent = allCollections.GroupBy(c => c.Parent?.Key ?? 0).ToDictionary(g => g.Key, g => g.ToList());

            // Filter list to collections with children
            List<VLTCollection> withChildren = allCollections.Where(c => groupedByParent.ContainsKey(c.Key)).OrderBy(c => c.Key).ToList();

            // Get list of top-level (no parent) collections
            List<VLTCollection> topLevel = allCollections.Where(c => c.Parent == null).ToList();

            List<uint> arrayElements = new List<uint>
            {
                0 // 0 represents the class root
            };

            // Collections with children are in the array and tree
            foreach (VLTCollection c in withChildren)
            {
                arrayElements.Add((uint)c.Key);
            }

            Dictionary<int, int> tree = new Dictionary<int, int>
            {
                { 0, topLevel.Count } // class root
            };

            int treeIdx = topLevel.Count;
            int arrIdx = 1;

            for (; arrIdx < arrayElements.Count; arrIdx++)
            {
                tree.Add(treeIdx, groupedByParent[arrayElements[arrIdx]].Count);

                treeIdx += tree[treeIdx];
            }

            Debug.Assert(tree.Count == arrayElements.Count);

            bw.Write(arrayElements.Count);
            ElementsPointer = bw.WritePointer();
            TreePointer = bw.WritePointer();
            EndPointer = bw.WritePointer();

            ElementsDest = bw.BaseStream.Position;

            foreach (uint arrayElement in arrayElements)
            {
                bw.Write(arrayElement);
            }

            TreeDest = bw.BaseStream.Position;

            foreach (KeyValuePair<int, int> i in tree)
            {
                bw.Write(i.Key);
                bw.Write(i.Value);
            }

            EndDest = bw.BaseStream.Position;
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            // do nothing
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            // do nothing as well
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(ElementsPointer, ElementsDest, false);
            vault.SaveContext.AddPointer(TreePointer, TreeDest, false);
            vault.SaveContext.AddPointer(EndPointer, EndDest, false);
        }

        public Static_Inorder_N_to_1(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public Static_Inorder_N_to_1(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}