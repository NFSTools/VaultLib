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
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace BurnoutConsole
{
    internal class Program
    {
        private static readonly string _gameId = "BURNOUT_PARADISE";

        private static void Main(string[] args)
        {
            HashManager.LoadDictionary("hashes.txt");

            // Prepare readers
            ExportFactory.SetClassLoadCreator<ClassLoad64>(_gameId);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad64>(_gameId);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(_gameId);
            ExportFactory.SetExportEntryCreator<ExportEntry64>(_gameId);
            ExportFactory.SetPointerCreator<PtrRef64>(_gameId);

            // Load data
            Database database = new Database(new DatabaseOptions(_gameId, DatabaseType.X64Database));
            List<string> fileList = new List<string> { "schema.bin", "XUSBHB1_AttribSys.bin" };
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
                BurnoutVaultPack bvp = new BurnoutVaultPack(Path.GetFileNameWithoutExtension(entry.Key));
                using FileStream fs = new FileStream(entry.Key + ".gen", FileMode.Create, FileAccess.Write);
                using BinaryWriter bw = new BinaryWriter(fs);

                bvp.Save(bw, entry.Value, new PackSavingOptions());
            }
        }

        private static IList<Vault> LoadFileToDB(Database database, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                BurnoutVaultPack bvp = new BurnoutVaultPack(Path.GetFileNameWithoutExtension(filePath));
                return bvp.Load(br, database, new PackLoadingOptions());
            }
        }
    }
}
