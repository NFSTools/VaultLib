// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 10:36 AM.

using System;
using System.IO;
using VaultLib.Core.Chunks;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.IO;

/// <summary>
///     Writes AttribSys-style chunks to a data stream.
/// </summary>
public class ChunkWriter<TKey> where TKey : struct, IKey<TKey>
{
    /// <summary>
    ///     Initializes the chunk writer with a backing <see cref="BinaryWriter" /> and <see cref="Vault{TKey}" />
    /// </summary>
    /// <param name="writer">The <see cref="BinaryWriter" /> instance that will write to the stream</param>
    /// <param name="writeContext">The <see cref="VaultWriteContext{TKey}" /> instance to provide to chunk instances</param>
    public ChunkWriter(BinaryWriter writer, VaultWriteContext<TKey> writeContext)
    {
        Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        WriteContext = writeContext ?? throw new ArgumentNullException(nameof(writeContext));
    }

    private BinaryWriter Writer { get; }

    private VaultWriteContext<TKey> WriteContext { get; }

    /// <summary>
    ///     Writes a chunk to the data stream.
    /// </summary>
    /// <param name="chunk">The chunk to write.</param>
    public void WriteChunk(ChunkBase<TKey> chunk)
    {
        var beginPos = Writer.BaseStream.Position;
        Writer.Write(chunk.Id);
        var sizePos = Writer.BaseStream.Position;
        Writer.Write(0);

        chunk.Write(WriteContext, Writer);

        var endPos = Writer.BaseStream.Position;

        Writer.BaseStream.Position = sizePos;
        Writer.Write((uint)(endPos - beginPos));
        Writer.BaseStream.Position = endPos;
    }
}