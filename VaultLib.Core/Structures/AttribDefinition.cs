// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 3:18 PM.

using System;
using System.Diagnostics;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Structures
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
            Flags = (DefinitionFlags) br.ReadByte();
            Alignment = 1 << br.ReadByte();

            if ((Flags & DefinitionFlags.kIsStatic) != 0)
            {
                Debug.Assert((Flags & DefinitionFlags.kInLayout) == 0, "(Flags & DefinitionFlags.kInLayout) == 0");
                // NOTE 11.01.19: apparently this is a false assumption. Cool.
                //Debug.Assert((Flags & DefinitionFlags.kArray) == 0, "(Flags & DefinitionFlags.kArray) == 0"); 
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