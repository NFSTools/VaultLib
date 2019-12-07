// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 9:28 PM.

using System;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Loading.Structures
{
    public class AttribVaultPackImage : IFileAccess
    {
        public AttribVaultPackHeader Header { get; set; }

        public List<AttribVaultPackEntry> Entries { get; set; }

        public void Read(Vault vault, BinaryReader br)
        {
            Entries = new List<AttribVaultPackEntry>();
            Header = new AttribVaultPackHeader();
            Header.Read(vault, br);

            for (var i = 0; i < Header.NumEntries; i++)
            {
                var entry = new AttribVaultPackEntry();
                entry.Read(vault, br);
                Entries.Add(entry);
            }
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            Header.Write(vault, bw);

            foreach (var entry in Entries)
            {
                entry.Write(vault, bw);
            }
        }
    }
}