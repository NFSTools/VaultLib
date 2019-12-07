// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 4:33 PM.

using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.ModernBase.Structures
{
    public class ExportEntry : IExportEntry
    {
        public void Read(Vault vault, BinaryReader br)
        {
            ID = br.ReadUInt32();
            Type = br.ReadUInt32();
            Size = br.ReadUInt32();
            Offset = br.ReadUInt32();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((uint)ID);
            bw.Write((uint)Type);
            bw.Write(Size);
            bw.Write(Offset);
        }

        public ulong ID { get; set; }
        public ulong Type { get; set; }
        public uint Size { get; set; }
        public uint Offset { get; set; }
    }
}