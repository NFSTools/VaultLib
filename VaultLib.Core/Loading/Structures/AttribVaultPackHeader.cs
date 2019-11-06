// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 9:29 PM.

using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Loading.Structures
{
    public class AttribVaultPackHeader : IFileAccess
    {
        public uint NumEntries { get; set; }
        public uint StringBlockOffset { get; set; }
        public uint StringBlockSize { get; set; }

        public void Read(Vault vault, BinaryReader br)
        {
            if (br.ReadUInt32() != 1262571606)
            {
                throw new InvalidDataException("Pack header invalid");
            }

            NumEntries = br.ReadUInt32();
            StringBlockOffset = br.ReadUInt32();
            StringBlockSize = br.ReadUInt32();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }
    }
}