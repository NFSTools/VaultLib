using System.Collections.Generic;

namespace VaultLib.Core.DB
{
    public class DatabaseLoadedFile
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }

        public List<Vault> Vaults { get; }

        public bool IsAlternateHeader { get; set; }
        
        public DatabaseLoadedFile()
        {
            Vaults = new List<Vault>();
        }
    }
}