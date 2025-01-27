// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/25/2019 @ 3:18 PM.

using System;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.ModernBase;

public class AttribDefinition32 : IAttribDefinition<Key32>
{
    public void Read(VaultReadContext<Key32> context, BinaryReader br)
    {
        Key = new Key32(br.ReadUInt32());
        Type = new Key32(br.ReadUInt32());
        Offset = br.ReadUInt16();
        Size = br.ReadUInt16();
        MaxCount = br.ReadUInt16();
        Flags = (DefinitionFlags)br.ReadByte();
        Alignment = 1 << br.ReadByte();
    }

    public void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        bw.Write(Key.Hash);
        bw.Write(Type.Hash);
        bw.Write(Offset);
        bw.Write(Size);
        bw.Write(MaxCount);
        bw.Write((byte)Flags);
        bw.Write((byte)Math.Log(Alignment, 2));
    }

    public Key32 Key { get; set; }
    public Key32 Type { get; set; }
    public ushort Offset { get; set; }
    public ushort Size { get; set; }
    public ushort MaxCount { get; set; }
    public DefinitionFlags Flags { get; set; }
    public int Alignment { get; set; }
}