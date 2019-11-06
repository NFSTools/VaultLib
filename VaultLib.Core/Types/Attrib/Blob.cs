// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/07/2019 @ 7:11 PM.

using System;
using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Types.Attrib
{
    [VLTTypeInfo("Attrib::Types::Blob")]
    public class Blob : VLTBaseType, IPointerObject
    {
        public enum CompressionID : uint
        {
            HUFF = 0x46465548,
            JDLZ = 0x5A4C444A,
            RAWW = 0x57574152
        }

        public class CompressedBlob : IFileAccess
        {
            public CompressionID Type { get; set; }

            public uint DecompressedLength { get; set; }

            private byte[] CompressedData { get; set; }

            public byte[] Data { get; set; }

            public void Read(Vault vault, BinaryReader br)
            {
                Type = (CompressionID)br.ReadUInt32();
                if (br.ReadByte() > 2)
                    throw new InvalidDataException("Invalid LZHeader version!");
                if (br.ReadByte() != 0x10)
                    throw new InvalidDataException("Invalid LZHeader size!");
                if (br.ReadUInt16() != 0)
                    throw new InvalidDataException("Expected 0 flags but got something else in LZHeader");
                DecompressedLength = br.ReadUInt32();
                var compLength = br.ReadInt32();
                if (Type == CompressionID.JDLZ) compLength -= 16;
                CompressedData = new byte[compLength];

                if (br.Read(CompressedData, 0, CompressedData.Length) != CompressedData.Length)
                {
                    throw new InvalidDataException("Failed to read " + CompressedData.Length + " bytes");
                }

                this.Data = new byte[DecompressedLength];
                this.DecompressData();

                CompressedData = null;
            }

            public void PrepareCompressedData()
            {
                byte[] compressed = new byte[Data.Length << 2];
                Compression.Compress(Data, ref compressed);
                CompressedData = compressed;
            }

            public void Write(Vault vault, BinaryWriter bw)
            {
                if (CompressedData == null)
                    throw new Exception("compressed data buffer is null");
                bw.Write(CompressedData);
            }

            public int GetLength()
            {
                return 16 + CompressedData.Length;
            }

            public bool HasData()
            {
                return Data != null;
            }

            private byte GetVersion()
            {
                switch (Type)
                {
                    case CompressionID.JDLZ:
                        return 2;
                    case CompressionID.HUFF:
                    case CompressionID.RAWW:
                        return 1;
                    default:
                        throw new Exception();
                }
            }

            private void DecompressData()
            {
                byte[] inData = new byte[16 + CompressedData.Length];
                Array.Copy(BitConverter.GetBytes((uint)Type), inData, 4);
                inData[5] = GetVersion();
                inData[6] = 0x10;
                inData[8] = (byte)(DecompressedLength & 0xff);
                inData[9] = (byte)((DecompressedLength >> 8) & 0xff);
                inData[10] = (byte)((DecompressedLength >> 16) & 0xff);
                inData[11] = (byte)((DecompressedLength >> 24) & 0xff);
                inData[12] = (byte)(CompressedData.Length & 0xff);
                inData[13] = (byte)((CompressedData.Length >> 8) & 0xff);
                inData[14] = (byte)((CompressedData.Length >> 16) & 0xff);
                inData[15] = (byte)((CompressedData.Length >> 24) & 0xff);
                Array.ConstrainedCopy(CompressedData, 0, inData, 16, CompressedData.Length);

                Compression.Decompress(inData, Data);
            }
        }

        public uint Length { get; set; }

        public CompressedBlob Data { get; set; }

        private uint _dataOffset;

        private long _dataPtrSrc;
        private long _dataPtrDst;

        public override void Read(Vault vault, BinaryReader br)
        {
            // Note regarding this sad excuse for a data structure: 
            // There are cases where the reported length is INCORRECT!
            // That's right - the data lies about itself. Wonderful, right?
            // This cost me a week of trying tons of different things. A week wasted,
            // because whatever failure of a process generated this decided that reporting the
            // proper length was not worth its due CPU instructions and execution time.
            // Great work, pipeline. Great work.
            Length = br.ReadUInt32();
            _dataOffset = br.ReadPointer(vault, false);
            Data = new CompressedBlob();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if (Data.HasData())
            {
                Data.PrepareCompressedData();
                bw.Write(Data.GetLength());
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
            if (_dataOffset != 0)
            {
                br.BaseStream.Position = _dataOffset;

                Data.Read(vault, br);
            }
        }

        public void WritePointerData(Vault vault, BinaryWriter bw)
        {
            if (Data.HasData())
            {
                _dataPtrDst = bw.BaseStream.Position;
                Data.Write(vault, bw);
            }
        }

        public void AddPointers(Vault vault)
        {
            vault.SaveContext.AddPointer(_dataPtrSrc, _dataPtrDst, false);
        }
    }
}