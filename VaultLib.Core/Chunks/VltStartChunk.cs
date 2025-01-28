// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:12 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Chunks;

public class VltStartChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    public override uint Id => 0x5374724E;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        //Debug.WriteLine("start");
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        bw.Write(new byte[8]);
    }
}