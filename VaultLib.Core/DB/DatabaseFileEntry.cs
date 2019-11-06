// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/03/2019 @ 7:22 AM.

using System.Collections.Generic;

namespace VaultLib.Core.DB
{
    public class DatabaseFileEntry
    {
        public string Path { get; set; }
        public string SavePath { get; set; }
        public List<Vault> Vaults { get; } = new List<Vault>();
    }
}