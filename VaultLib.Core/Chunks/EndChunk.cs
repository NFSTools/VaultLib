using System;
using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Chunks;

public class EndChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    public override uint Id => 0x456E6443;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        //Debug.WriteLine("end");
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        if ((bw.BaseStream.Position - 8) % 0x10 != 0) throw new Exception();

        bw.Write(new byte[8]);
    }
}