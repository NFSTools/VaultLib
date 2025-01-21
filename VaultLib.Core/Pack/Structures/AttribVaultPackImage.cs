// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 9:28 PM.

using System.Collections.Generic;
using System.IO;

namespace VaultLib.Core.Pack.Structures;

public class AttribVaultPackImage
{
    public AttribVaultPackHeader Header { get; set; }

    public List<AttribVaultPackEntry> Entries { get; set; }

    public void Read(BinaryReader br)
    {
        Entries = new List<AttribVaultPackEntry>();
        Header = new AttribVaultPackHeader();
        Header.Read(br);

        for (var i = 0; i < Header.NumEntries; i++)
        {
            var entry = new AttribVaultPackEntry();
            entry.Read(br);
            Entries.Add(entry);
        }
    }

    public void Write(BinaryWriter bw)
    {
        Header.Write(bw);

        foreach (var entry in Entries)
        {
            entry.Write(bw);
        }
    }
}