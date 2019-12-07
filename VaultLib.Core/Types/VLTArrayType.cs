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
using VaultLib.Core.Types.EA.Reflection;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VLTArrayType : VLTBaseType, IReferencesStrings, IReferencesCollections
    {
        public VLTArrayType(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field,
            collection)
        {
        }

        public VLTArrayType(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }

        public ushort FieldSize { get; set; }

        public ushort Capacity { get; set; }

        public int ItemAlignment { get; set; }

        public Type ItemType { get; set; }

        public VLTBaseType[] Items { get; set; }

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
            Items = new VLTBaseType[count];
            FieldSize = br.ReadUInt16();

            // NOTE: this is 0x8000 when Attrib::Types::Vector4 is in use. not sure why. 0 otherwise
            var readUInt16 = br.ReadUInt16();

            if (readUInt16 != 0)
            {
                Debug.WriteLine("CHECK: expected 0 for EncodedTypePad, but got {0}: type {1}", readUInt16, Field.TypeName);
            }

            for (var i = 0; i < Items.Length; i++)
            {
                Items[i] = TypeRegistry.ConstructInstance(ItemType, Class, Field, Collection);
                //Items[i] = (VLTBaseType)Activator.CreateInstance(ItemType, Class, Field, Collection);

                br.AlignReader(ItemAlignment);

                if (Items[i] is VLTUnknown unknown) unknown.Size = FieldSize;

                Items[i].Read(vault, br);
            }

            br.BaseStream.Position += (Capacity - count) * FieldSize;
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Capacity);
            bw.Write((ushort)Items.Length);
            bw.Write(FieldSize);
            bw.Write((ushort)(1 << (Field.Alignment - 1)));

            foreach (var t in Items)
            {
                bw.AlignWriter(ItemAlignment);
                var start = bw.BaseStream.Position;
                t.Write(vault, bw);
                if (!(t is PrimitiveTypeBase)) Debug.Assert(bw.BaseStream.Position - start == FieldSize);
            }

            for (var i = 0; i < Capacity - Items.Length; i++)
            {
                bw.AlignWriter(ItemAlignment);
                bw.Write(new byte[FieldSize]);
            }
        }

        public override string ToString()
        {
            return string.Join<VLTBaseType>(" | ", Items);
        }
    }
}