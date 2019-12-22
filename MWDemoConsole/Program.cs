using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Hashing;
using VaultLib.Core.Pack;
using VaultLib.LegacyBase.Exports;
using VaultLib.LegacyBase.Structures;

namespace MWDemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database(new DatabaseOptions("MW_PS2", DatabaseType.X64Database));
            HashManager.LoadDictionary("hashes.txt");

            // Prepare readers
            ExportFactory.SetClassLoadCreator<ClassLoad64>("MW_PS2");
            ExportFactory.SetCollectionLoadCreator<CollectionLoad64>("MW_PS2");
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>("MW_PS2");
            ExportFactory.SetExportEntryCreator<ExportEntry64>("MW_PS2");
            ExportFactory.SetPointerCreator<PtrRef64>("MW_PS2");
            using (new DatabaseLoadingWrapper(database))
            {
                List<string> fileList = new List<string> { "ATTRIBUTES.BIN", "GAMEPLAY.BIN" };
                Dictionary<string, IList<Vault>> fileDictionary = fileList.ToDictionary(c => c, c => LoadFileToDB(database, c));
            }
        }

        private static IList<Vault> LoadFileToDB(Database database, string file)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(file)))
            {
                StandardVaultPack vaultPack = new StandardVaultPack();
                return vaultPack.Load(br, database, new PackLoadingOptions());
            }
        }
    }
}
