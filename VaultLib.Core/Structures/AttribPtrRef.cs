﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 5:12 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Structures
{
    public class AttribPtrRef : IPtrRef
    {
        public void Read(Vault vault, BinaryReader br)
        {
            FixupOffset = br.ReadUInt32();
            PtrType = (EPtrRefType)br.ReadUInt16();
            Index = br.ReadUInt16();
            Destination = br.ReadUInt32();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(FixupOffset);
            bw.Write((ushort)PtrType);
            bw.Write(Index);
            bw.Write(Destination);
        }

        public uint FixupOffset { get; set; }
        public EPtrRefType PtrType { get; set; }
        public ushort Index { get; set; }
        public uint Destination { get; set; }
    }
}