// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:33 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.ModernBase.Structures;

public class ExportEntry32 : IExportEntry<Key32>
{
    public void Read(VaultReadContext<Key32> context, BinaryReader br)
    {
        Id = new Key32(br.ReadUInt32());
        Type = new Key32(br.ReadUInt32());
        Size = br.ReadUInt32();
        Offset = br.ReadUInt32();
    }

    public void Write(VaultWriteContext<Key32> context, BinaryWriter bw)
    {
        bw.Write(Id.Hash);
        bw.Write(Type.Hash);
        bw.Write(Size);
        bw.Write(Offset);
    }

    public Key32 Id { get; set; }
    public Key32 Type { get; set; }
    public uint Size { get; set; }
    public uint Offset { get; set; }
}