// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:32 PM.

using System;
using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Structures
{
    public class ChunkBlockHeader : IFileAccess
    {
        public long Offset { get; set; }

        public uint ID { get; set; }

        public uint Size { get; set; }

        public long EndOffset => Offset + Size + 8;

        public void Read(Vault vault, BinaryReader br)
        {
            Offset = br.BaseStream.Position;
            ID = br.ReadUInt32();
            Size = br.ReadUInt32() - 8;

            if (Offset + Size + 8 > br.BaseStream.Length)
                throw new InvalidDataException($"Overflowing chunk detected @ base+0x{Offset:X}");
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public void GoToEnd(Stream stream)
        {
            stream.Seek(EndOffset, SeekOrigin.Begin);
        }
    }
}