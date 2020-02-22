using System;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib
{
    public abstract class BaseBlob : VLTBaseType, IPointerObject
    {
        protected BaseBlob(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        protected BaseBlob(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public byte[] Data { get; set; }

        protected int Length { get; set; }

        private byte[] _preparedData;

        private uint _dataOffset;
        private long _dataPtrDst;

        private long _dataPtrSrc;

        public override void Read(Vault vault, BinaryReader br)
        {
            Length = br.ReadInt32();

            if (Length < 0)
            {
                throw new InvalidDataException("Blob length cannot be less than 0");
            }

            _dataOffset = br.ReadPointer();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (Data != null)
            {
                _preparedData = PrepareData() ?? throw new Exception("PrepareData() returned null.");
                bw.Write(_preparedData.Length);
            }
            else
            {
                bw.Write(0);
            }

            _dataPtrSrc = bw.BaseStream.Position;
            bw.Write(0);
        }

        public void ReadPointerData(Vault vault, BinaryReader br)
        {
            Data = ReadData(br);
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            if (_preparedData != null)
            {
                _dataPtrDst = bw.BaseStream.Position;
                bw.Write(_preparedData);
            }
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_dataPtrSrc, _dataPtrDst, false);
        }

        protected abstract byte[] PrepareData();

        protected virtual byte[] ReadData(BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(Length);

            if (bytes.Length != Length)
            {
                throw new InvalidDataException($"Expected {Length} bytes but got {bytes.Length}");
            }

            return bytes;
        }
    }
}