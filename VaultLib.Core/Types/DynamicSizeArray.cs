// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 5:40 PM.

using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class DynamicSizeArray<T> : VltBaseType, IPointerObject where T : VltBaseType
    {
        private long _dstPtr;

        private uint _pointer;
        private long _srcPtr;

        public DynamicSizeArray(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field,
            collection)
        {
        }

        public DynamicSizeArray(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public T[] Items { get; set; }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            var databaseTypeRegistry = context.Database.TypeRegistry;
            
            br.BaseStream.Position = _pointer;
            for (var i = 0; i < Items.Length; i++)
            {
                Items[i] = (T)databaseTypeRegistry.ConstructInstance(typeof(T), Class, Field, Collection);
                Items[i].Read(context, br);
            }
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _dstPtr = bw.BaseStream.Position;
            foreach (var vltBaseType in Items) vltBaseType.Write(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            context.AddPointer(_srcPtr, _dstPtr, false);
        }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _pointer = br.ReadUInt32();
            Items = new T[br.ReadInt32()];
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _srcPtr = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(Items.Length);
        }
    }
}