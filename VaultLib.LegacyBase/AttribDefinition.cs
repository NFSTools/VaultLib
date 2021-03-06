﻿// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 3:18 PM.

using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.LegacyBase
{
    public class AttribDefinition : IAttribDefinition
    {
        public void Read(Vault vault, BinaryReader br)
        {
            Key = br.ReadUInt32();
            Type = br.ReadUInt32();
            Offset = br.ReadUInt16();
            Size = br.ReadUInt16();
            MaxCount = br.ReadUInt16();
            Flags = (DefinitionFlags)br.ReadByte();
            Alignment = 1 << br.ReadByte();

            if ((Flags & DefinitionFlags.IsStatic) != 0)
            {
                throw new InvalidDataException("IsStatic cannot be set in legacy AttribDefinition");
            }
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((uint)Key);
            bw.Write((uint)Type);
            bw.Write(Offset);
            bw.Write(Size);
            bw.Write(MaxCount);
            bw.Write((byte)Flags);
            bw.Write((byte)Math.Log(Alignment, 2));
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