using CoreLibraries.IO;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Hashing;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace SkateConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream binStream = new MemoryStream();
            MemoryStream vltStream = new MemoryStream();

            using (FileStream fs = new FileStream("skater.bin", FileMode.Open))
            {
                fs.CopyTo(binStream);
            }

            using (FileStream fs = new FileStream("skater.vlt", FileMode.Open))
            {
                fs.CopyTo(vltStream);
            }

            binStream.Position = vltStream.Position = 0;

            Vault vault = new Vault("skater")
            {
                BinStream = binStream,
                VltStream = vltStream,
                IsPrimaryVault = true
            };

            Database database = new Database(new DatabaseOptions("SKATER", DatabaseType.X64Database));
            HashManager.LoadDictionary("hashes.txt");

            // Prepare readers
            ExportFactory.SetClassLoadCreator<ClassLoad64>("SKATER");
            ExportFactory.SetCollectionLoadCreator<CollectionLoad64>("SKATER");
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>("SKATER");
            ExportFactory.SetExportEntryCreator<ExportEntry64>("SKATER");
            ExportFactory.SetPointerCreator<PtrRef64>("SKATER");

            using (VaultLoadingWrapper loadingWrapper = new VaultLoadingWrapper(vault, ByteOrder.Big))
            {
                database.LoadVault(vault, loadingWrapper);
            }

            database.CompleteLoad();

            Debug.WriteLine("Listing types:");
            foreach (DatabaseTypeInfo typeInfo in database.Types.OrderBy(t => t.Name))
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
        }
    }
}
