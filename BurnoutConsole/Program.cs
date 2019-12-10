using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using CoreLibraries.GameUtilities;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Hashing;
using VaultLib.Core.Loading;
using VaultLib.Core.Structures;
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

            using (FileStream fs = new FileStream(@"schema.bin", FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                BurnoutVaultPack bvp = new BurnoutVaultPack("schema");
                bvp.Load(br, database, new PackLoadingOptions());
            }

            database.CompleteLoad();
            TypeRegistry.ListUnknownTypes();

            foreach (DatabaseTypeInfo typeInfo in database.Types)
            {
                Debug.WriteLine("TYPE: {0} (size {1})", typeInfo.Name, typeInfo.Size);
            }
        }
    }
}
