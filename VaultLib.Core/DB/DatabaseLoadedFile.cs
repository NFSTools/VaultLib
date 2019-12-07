using System.Collections.Generic;

namespace VaultLib.Core.DB
{
    public class DatabaseLoadedFile
    {
        public DatabaseLoadedFile()
        {
            Vaults = new List<Vault>();
        }

        public List<Vault> Vaults { get; }
    }
}