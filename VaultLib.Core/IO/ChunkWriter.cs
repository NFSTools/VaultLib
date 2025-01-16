// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 10:36 AM.

using System;
using System.IO;
using VaultLib.Core.Chunks;

namespace VaultLib.Core.IO
{
    /// <summary>
    ///     Writes AttribSys-style chunks to a data stream.
    /// </summary>
    public class ChunkWriter
    {
        /// <summary>
        ///     Initializes the chunk writer with a backing <see cref="BinaryWriter" /> and <see cref="VaultLib.Core.Vault" />
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter" /> instance that will write to the stream</param>
        /// <param name="saveContext">The <see cref="VaultLib.Core.VaultSaveContext" /> instance to provide to chunk instances</param>
        public ChunkWriter(BinaryWriter writer, VaultSaveContext saveContext)
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            SaveContext = saveContext ?? throw new ArgumentNullException(nameof(saveContext));
        }

        private BinaryWriter Writer { get; }

        private VaultSaveContext SaveContext { get; }

        /// <summary>
        ///     Writes a chunk to the data stream.
        /// </summary>
        /// <param name="chunk">The chunk to write.</param>
        public void WriteChunk(ChunkBase chunk)
        {
            var beginPos = Writer.BaseStream.Position;
            Writer.Write(chunk.Id);
            var sizePos = Writer.BaseStream.Position;
            Writer.Write(0);

            chunk.Write(SaveContext, Writer);

            var endPos = Writer.BaseStream.Position;

            Writer.BaseStream.Position = sizePos;
            Writer.Write((uint)(endPos - beginPos));
            Writer.BaseStream.Position = endPos;
        }
    }
}