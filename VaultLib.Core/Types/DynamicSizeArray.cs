// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:40 PM.

using System;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class DynamicSizeArray<T> : VLTBaseType, IPointerObject where T : VLTBaseType
    {
        private long _dstPtr;

        private uint _pointer;
        private long _srcPtr;

        public DynamicSizeArray(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field,
            collection)
        {
        }

        public DynamicSizeArray(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }

        public T[] Items { get; set; }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            br.BaseStream.Position = _pointer;
            for (var i = 0; i < Items.Length; i++)
            {
                Items[i] = (T) Activator.CreateInstance(typeof(T), Class, Field, Collection);
                Items[i].Read(vault, br);
            }
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _dstPtr = bw.BaseStream.Position;
            foreach (var vltBaseType in Items) vltBaseType.Write(vault, bw);
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_srcPtr, _dstPtr, false);
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            _pointer = br.ReadUInt32();
            Items = new T[br.ReadInt32()];
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _srcPtr = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(Items.Length);
        }
    }
}