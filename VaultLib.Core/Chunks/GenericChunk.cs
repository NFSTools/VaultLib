// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:58 PM.

using System.IO;

namespace VaultLib.Core.Chunks
{
    public class GenericChunk : ChunkBase
    {
        public GenericChunk(uint id)
        {
            ID = id;
        }

        public override void Read(Vault vault, BinaryReader br)
        {
            //Debug.WriteLine("UNKNOWN CHUNK of {0} bytes - text {2} ({1:X8})", this.Size, this.ID, Encoding.ASCII.GetString(BitConverter.GetBytes(this.ID).Reverse().ToArray()));
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }

        public override uint ID { get; }
        public override uint Size { get; set; }
        public override long Offset { get; set; }
    }
}