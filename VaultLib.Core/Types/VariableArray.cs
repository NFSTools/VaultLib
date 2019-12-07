// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/26/2019 @ 8:20 PM.

using System.Diagnostics;
using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types
{
    public class VariableArray : IFileAccess, IPointerObject
    {
        private uint _mArray;
        private long _ptrDst;

        private long _ptrSrc;
        public float[] Data { get; set; }

        public void Read(Vault vault, BinaryReader br)
        {
            _mArray = br.ReadPointer();
            Debug.Assert(_mArray != 0);
            var mLength = br.ReadUInt32();

            Data = new float[mLength];
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            _ptrSrc = bw.BaseStream.Position;
            bw.Write(0);
            bw.Write(Data.Length);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            br.BaseStream.Position = _mArray;

            for (var i = 0; i < Data.Length; i++) Data[i] = br.ReadSingle();
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            _ptrDst = bw.BaseStream.Position;

            foreach (var f in Data) bw.Write(f);
        }

        public void AddPointers(Vault vault)
        {
            Debug.Assert(_ptrSrc != 0 && _ptrDst != 0);
            vault.SaveContext.AddPointer(_ptrSrc, _ptrDst, false);
        }
    }
}