// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:29 PM.

using System.IO;
using VaultLib.Core.Chunks;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Structures;

namespace VaultLib.Core.IO;

/// <summary>
///     Reads chunks from a data stream
/// </summary>
public class ChunkReader<TKey> where TKey : struct, IKey<TKey>
{
    public ChunkReader(BinaryReader reader)
    {
        Reader = reader;
    }

    public BinaryReader Reader { get; }

    public ChunkBase<TKey> NextChunk()
    {
        var header = new ChunkBlockHeader();
        header.Read(Reader);
        ChunkBase<TKey> chunk;

        switch (header.ID)
        {
            case 0x53747245:
                chunk = new BinStringsChunk<TKey>();
                break;
            case 0x5374724E:
                chunk = new VltStartChunk<TKey>();
                break;
            case 0x456E6443:
                chunk = new EndChunk<TKey>();
                break;
            case 0x56657273:
                chunk = new VltVersionChunk<TKey>();
                break;
            case 0x4465704E:
                chunk = new VltDependencyChunk<TKey>();
                break;
            case 0x4578704E:
                chunk = new VltExportChunk<TKey>();
                break;
            case 0x5074724E:
                chunk = new VltPointersChunk<TKey>();
                break;
            default:
                chunk = new GenericChunk<TKey>(header.ID);
                break;
        }

        chunk.Offset = header.Offset;
        chunk.Size = header.Size + 8;

        return chunk;
    }
}