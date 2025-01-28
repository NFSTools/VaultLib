using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Chunks;

public class VltVersionChunk<TKey> : ChunkBase<TKey> where TKey : struct, IKey<TKey>
{
    public override uint Id => 0x56657273;
    public override uint Size { get; set; }
    public override long Offset { get; set; }

    public override void Read(VaultReadContext<TKey> context, BinaryReader br)
    {
        context.Vault.Version = br.ReadUInt64();

        //Debug.WriteLine("VLT version is: {0:X16}", Version);
    }

    public override void Write(VaultWriteContext<TKey> context, BinaryWriter bw)
    {
        bw.Write(context.Vault.Version);
    }
}