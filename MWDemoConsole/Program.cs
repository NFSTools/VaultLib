using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
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
            HashManager.LoadDictionary("hashes.txt");

            // Prepare readers
            ExportFactory.SetClassLoadCreator<ClassLoad64>("MW_PS2");
            ExportFactory.SetCollectionLoadCreator<CollectionLoad64>("MW_PS2");
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad64>("MW_PS2");
            ExportFactory.SetExportEntryCreator<ExportEntry64>("MW_PS2");
            ExportFactory.SetPointerCreator<PtrRef64>("MW_PS2");

            // Load data
            Database database = new Database(new DatabaseOptions("MW_PS2", DatabaseType.X64Database));
            List<string> fileList = new List<string> { "ATTRIBUTES.BIN", "GAMEPLAY.BIN" };
            Dictionary<string, IList<Vault>> fileDictionary = fileList.ToDictionary(c => c, c => LoadFileToDB(database, c));

            database.CompleteLoad();
            TypeRegistry.ListUnknownTypes();

            Debug.WriteLine("Listing types:");
            foreach (DatabaseTypeInfo typeInfo in database.Types)
            {
                Debug.WriteLine("\t{0} (size {1})", typeInfo.Name, typeInfo.Size);
            }

            Debug.WriteLine("Listing classes:");
            foreach (VltClass vltClass in database.Classes.OrderBy(c => c.Name))
            {
                Debug.WriteLine("\t{0} ({1} fields)", vltClass.Name, vltClass.Fields.Count);

                foreach (VltClassField field in vltClass.Fields.Values.OrderBy(f => f.Name))
                {
                    Debug.WriteLine("\t\t{0} ({1}) - flags: {2}", field.Name, field.TypeName, field.Flags);
                }
            }

            Debug.WriteLine("re-saving");
            foreach (var entry in fileDictionary)
            {
                StandardVaultPack bvp = new StandardVaultPack();
                using FileStream fs = new FileStream(entry.Key + ".gen", FileMode.Create, FileAccess.Write);
                using BinaryWriter bw = new BinaryWriter(fs);

                bvp.Save(bw, entry.Value, new PackSavingOptions());
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
