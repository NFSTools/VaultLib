using System;
using System.IO;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.Utils;

namespace VaultLib.Frameworks.Speed.Attrib
{
    public class CompressedBlob : IFileAccess
    {
        public enum CompressionId : uint
        {
            Huffman = 0x46465548,
            LempelZiv = 0x5A4C444A,
            Raw = 0x57574152
        }

        public CompressionId Type { get; set; }

        public uint DecompressedLength { get; set; }

        public byte[] CompressedData { get; set; }

        public byte[] Data { get; set; }

        public void Read(Vault vault, BinaryReader br)
        {
            Type = (CompressionId)br.ReadUInt32();
            if (br.ReadByte() > 2)
                throw new InvalidDataException("Invalid LZHeader version!");
            if (br.ReadByte() != 0x10)
                throw new InvalidDataException("Invalid LZHeader size!");
            if (br.ReadUInt16() != 0)
                throw new InvalidDataException("Expected 0 flags but got something else in LZHeader");
            DecompressedLength = br.ReadUInt32();
            var compLength = br.ReadInt32();
            if (Type == CompressionId.LempelZiv) compLength -= 16;
            CompressedData = new byte[compLength];

            if (br.Read(CompressedData, 0, CompressedData.Length) != CompressedData.Length)
                throw new InvalidDataException("Failed to read " + CompressedData.Length + " bytes");

            Data = new byte[DecompressedLength];
            DecompressData();

            CompressedData = null;
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            if (CompressedData == null)
                throw new Exception("compressed data buffer is null");
            bw.Write(CompressedData);
        }

        public void PrepareCompressedData()
        {
            var compressed = new byte[Data.Length << 2];
            Compression.Compress(Data, ref compressed);
            CompressedData = compressed;
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
                case CompressionId.LempelZiv:
                    return 2;
                case CompressionId.Huffman:
                case CompressionId.Raw:
                    return 1;
                default:
                    throw new Exception();
            }
        }

        private void DecompressData()
        {
            var inData = new byte[16 + CompressedData.Length];
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
}