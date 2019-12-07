// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:29 PM.

using System.IO;
using VaultLib.Core.Chunks;
using VaultLib.Core.Structures;

namespace VaultLib.Core.IO
{
    /// <summary>
    ///     Reads chunks from a data stream
    /// </summary>
    public class ChunkReader
    {
        public ChunkReader(BinaryReader reader)
        {
            Reader = reader;
        }

        public BinaryReader Reader { get; }

        public ChunkBase NextChunk(Vault vault)
        {
            var header = new ChunkBlockHeader();
            header.Read(vault, Reader);
            ChunkBase chunk = null;

            switch (header.ID)
            {
                case 0x53747245:
                    chunk = new BINStringsChunk();
                    break;
                case 0x5374724E:
                    chunk = new VLTStartChunk();
                    break;
                case 0x456E6443:
                    chunk = new EndChunk();
                    break;
                case 0x56657273:
                    chunk = new VLTVersionChunk();
                    break;
                case 0x4465704E:
                    chunk = new VLTDependencyChunk();
                    break;
                case 0x4578704E:
                    chunk = new VLTExportChunk();
                    break;
                case 0x5074724E:
                    chunk = new VLTPointersChunk();
                    break;
                default:
                    chunk = new GenericChunk(header.ID);
                    break;
            }

            chunk.Offset = header.Offset;
            chunk.Size = header.Size + 8;

            return chunk;
        }
    }
}