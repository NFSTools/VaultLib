﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 3:58 PM.

using System;
using System.IO;

namespace VaultLib.Core.Chunks
{
    public class GenericChunk : ChunkBase
    {
        public GenericChunk(uint id)
        {
            Id = id;
        }

        public override uint Id { get; }
        public override uint Size { get; set; }
        public override long Offset { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            //Debug.WriteLine("UNKNOWN CHUNK of {0} bytes - text {2} ({1:X8})", this.Size, this.ID, Encoding.ASCII.GetString(BitConverter.GetBytes(this.ID).Reverse().ToArray()));
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}