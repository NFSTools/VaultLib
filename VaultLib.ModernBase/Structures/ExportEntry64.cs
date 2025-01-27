// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:33 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.ModernBase.Structures;

public class ExportEntry64 : IExportEntry<Key64>
{
    public void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        Id = new Key64(br.ReadUInt64());
        Type = new Key64(br.ReadUInt64());
        Size = br.ReadUInt32();
        Offset = br.ReadUInt32();
    }

    public void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.Write(Id.Hash);
        bw.Write(Type.Hash);
        bw.Write(Size);
        bw.Write(Offset);
    }

    public Key64 Id { get; set; }
    public Key64 Type { get; set; }
    public uint Size { get; set; }
    public uint Offset { get; set; }
}