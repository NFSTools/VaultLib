using System;
using System.IO;

namespace VaultLib.Core.Chunks
{
    public class EndChunk : ChunkBase
    {
        public override void Read(Vault vault, BinaryReader br)
        {
            //Debug.WriteLine("end");
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            if ((bw.BaseStream.Position - 8) % 0x10 != 0)
            {
                throw new Exception();
            }

            bw.Write(new byte[8]);
        }

        public override uint ID => 0x456E6443;
        public override uint Size { get; set; }
        public override long Offset { get; set; }
    }
}
