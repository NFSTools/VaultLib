// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:58 PM.

using System;
using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Chunks;

public class GenericChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    public GenericChunk(uint id)
    {
        Id = id;
    }

    public override uint Id { get; }
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        //Debug.WriteLine("UNKNOWN CHUNK of {0} bytes - text {2} ({1:X8})", this.Size, this.ID, Encoding.ASCII.GetString(BitConverter.GetBytes(this.ID).Reverse().ToArray()));
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        throw new NotImplementedException();
    }
}