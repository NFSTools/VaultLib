// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 9:29 PM.

using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Pack.Structures
{
    public class AttribVaultPackEntry : IFileAccess
    {
        public uint VaultNameOffset { get; set; }
        public uint BinSize { get; set; }
        public uint VltSize { get; set; }
        public uint BinOffset { get; set; }
        public uint VltOffset { get; set; }

        public void Read(Vault vault, BinaryReader br)
        {
            VaultNameOffset = br.ReadUInt32();
            BinSize = br.ReadUInt32();
            VltSize = br.ReadUInt32();
            BinOffset = br.ReadUInt32();
            VltOffset = br.ReadUInt32();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(VaultNameOffset);
            bw.Write(BinSize);
            bw.Write(VltSize);
            bw.Write(BinOffset);
            bw.Write(VltOffset);
        }
    }
}