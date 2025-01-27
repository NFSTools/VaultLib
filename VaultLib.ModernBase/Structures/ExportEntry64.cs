// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:33 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.ModernBase.Structures;

public class ExportEntry64 : IExportEntry<ulong>
{
    public void Read(VaultReadContext<ulong> context, BinaryReader br)
    {
        Id = br.ReadUInt64();
        Type = br.ReadUInt64();
        Size = br.ReadUInt32();
        Offset = br.ReadUInt32();
    }

    public void Write(VaultWriteContext<ulong> context, BinaryWriter bw)
    {
        bw.Write(Id);
        bw.Write(Type);
        bw.Write(Size);
        bw.Write(Offset);
    }

    public ulong Id { get; set; }
    public ulong Type { get; set; }
    public uint Size { get; set; }
    public uint Offset { get; set; }
}