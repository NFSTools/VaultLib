// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:56 PM.

using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    /// <summary>
    ///     Helper class for reading data types through a pointer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VltPointerContainer<T> : VltBaseType, IVltPointerObject where T : VltBaseType
    {
        private uint _pointer;
        private long _ptrDst;

        private long _ptrSrc;

        public T Value { get; set; }

        public void ReadPointerData(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            br.BaseStream.Position = _pointer;
            Value = (T)context.Database.TypeRegistry.ConstructTypeInstance(typeof(T),
                fieldContext.Field);
            Value.Read(context, fieldContext, br);

            if (Value is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.ReadPointerData(context, fieldContext, br);
            }
        }

        public void WritePointerData(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _ptrDst = bw.BaseStream.Position;
            Value.Write(context, fieldContext, bw);

            if (Value is IVltPointerObject vltPointerObject)
            {
                vltPointerObject.WritePointerData(context, fieldContext, bw);
            }
        }

        public void AddPointers(VaultWriteContext context, FieldReadWriteContext fieldContext)
        {
            context.AddPointer(_ptrSrc, _ptrDst, false);
        }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            _pointer = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            _ptrSrc = bw.BaseStream.Position;
            bw.Write(0);
        }
    }
}