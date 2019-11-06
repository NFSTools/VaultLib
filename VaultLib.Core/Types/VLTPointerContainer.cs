// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/19/2019 @ 4:56 PM.

using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    /// <summary>
    /// Helper class for reading data types through a pointer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VLTPointerContainer<T> : VLTBaseType, IPointerObject where T : VLTBaseType, new()
    {
        public T Value { get; set; }

        private uint _pointer;

        private long _ptrSrc;
        private long _ptrDst;

        public override void Read(Vault vault, BinaryReader br)
        {
            _pointer = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            _ptrSrc = bw.BaseStream.Position;
            bw.Write(0);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            br.BaseStream.Position = _pointer;
            Value = new T();
            Value.Read(vault, br);

            if (Value is IPointerObject pointerObject)
            {
                pointerObject.ReadPointerData(vault, br);
            }
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _ptrDst = bw.BaseStream.Position;
            Value.Write(vault, bw);

            if (Value is IPointerObject pointerObject)
            {
                pointerObject.WritePointerData(vault, bw);
            }
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_ptrSrc, _ptrDst, false);
        }
    }
}