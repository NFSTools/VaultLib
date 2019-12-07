// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 3:18 PM.

using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.LegacyBase
{
    public class AttribDefinition64 : IAttribDefinition
    {
        public void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt64();
            Type = br.ReadUInt64();
            Offset = br.ReadUInt16();
            Size = br.ReadUInt16();
            MaxCount = br.ReadUInt16();
            Flags = (DefinitionFlags) br.ReadByte();
            Alignment = 1 << br.ReadByte();

            if ((Flags & DefinitionFlags.kIsStatic) != 0)
            {
                throw new InvalidDataException("IsStatic cannot be set in legacy AttribDefinition");
            }
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Key);
            bw.Write(Type);
            bw.Write(Offset);
            bw.Write(Size);
            bw.Write(MaxCount);
            bw.Write((byte)Flags);
            bw.Write((byte) Math.Log(Alignment, 2));
        }

        public ulong Key { get; set; }
        public ulong Type { get; set; }
        public ushort Offset { get; set; }
        public ushort Size { get; set; }
        public ushort MaxCount { get; set; }
        public DefinitionFlags Flags { get; set; }
        public int Alignment { get; set; }
    }
}