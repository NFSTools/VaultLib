// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:12 PM.

using System.IO;

namespace VaultLib.Core.Chunks
{
    public class VLTStartChunk : ChunkBase
    {
        public override uint ID => 0x5374724E;
        public override uint Size { get; set; }
        public override long Offset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            //Debug.WriteLine("start");
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(new byte[8]);
        }
    }
}