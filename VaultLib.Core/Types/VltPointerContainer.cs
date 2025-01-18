// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:56 PM.

using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    /// <summary>
    ///     Helper class for reading data types through a pointer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VltPointerContainer<T> : VltBaseType, IPointerObject where T : VltBaseType
    {
        private uint _pointer;
        private long _ptrDst;

        private long _ptrSrc;

        public VltPointerContainer(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field,
            collection)
        {
        }

        public VltPointerContainer(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public T Value { get; set; }

        public void ReadPointerData(VaultLoadContext context, BinaryReader br)
        {
            br.BaseStream.Position = _pointer;
            Value = (T)context.Database.TypeRegistry.ConstructInstance(typeof(T), Class, Field, Collection);
            Value.Read(context, br);

            if (Value is IPointerObject pointerObject) pointerObject.ReadPointerData(context, br);
        }

        public void WritePointerData(VaultSaveContext context, BinaryWriter bw)
        {
            _ptrDst = bw.BaseStream.Position;
            Value.Write(context, bw);

            if (Value is IPointerObject pointerObject) pointerObject.WritePointerData(context, bw);
        }

        public void AddPointers(VaultSaveContext context)
        {
            context.AddPointer(_ptrSrc, _ptrDst, false);
        }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            _pointer = br.ReadUInt32();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            _ptrSrc = bw.BaseStream.Position;
            bw.Write(0);
        }
    }
}