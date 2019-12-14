// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:49 PM.

using System;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VLTListContainer<T> : VLTBaseType, IPointerObject where T : VLTBaseType
    {
        private long _dstPtr;

        private uint _pointer;

        private long _srcPtr;

        public VLTListContainer(VltClass @class, VltClassField field, VltCollection collection, int count) : base(
            @class, field, collection)
        {
            Items = new List<T>(count);
        }

        public VLTListContainer(VltClass @class, VltClassField field, int count) : this(@class, field, null, count)
        {
        }

        public List<T> Items { get; }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            br.BaseStream.Position = _pointer;

            for (var i = 0; i < Items.Capacity; i++)
            {
                var item = (T) TypeRegistry.ConstructInstance(typeof(T), Class, Field, Collection);
                //var item = (T) Activator.CreateInstance(typeof(T), Class, Field, Collection);
                item.Read(vault, br);
                Items.Add(item);
            }
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _dstPtr = bw.BaseStream.Position;

            foreach (var item in Items) item.Write(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_srcPtr, _dstPtr, false);
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            _pointer = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _srcPtr = bw.BaseStream.Position;
            bw.Write(0);
        }
    }
}