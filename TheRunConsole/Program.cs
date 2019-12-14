using System;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Exports;
using VaultLib.Core.Exports.Implementations;
using VaultLib.Core.Hashing;
using VaultLib.Core.Structures;
using VaultLib.ModernBase.Exports;
using VaultLib.ModernBase.Structures;

namespace TheRunConsole
{
    class Program
    {
        private static string _gameId = "THE_RUN";

        static void Main(string[] args)
        {
            HashManager.LoadDictionary("hashes.txt");

            // Register types
            TypeRegistry.RegisterAssemblyTypes(typeof(Program).Assembly, _gameId);

            // Prepare readers
            ExportFactory.SetClassLoadCreator<ClassLoad>(_gameId);
            ExportFactory.SetCollectionLoadCreator<CollectionLoad>(_gameId);
            ExportFactory.SetDatabaseLoadCreator<DatabaseLoad>(_gameId);
            ExportFactory.SetExportEntryCreator<ExportEntry>(_gameId);
            ExportFactory.SetPointerCreator<AttribPtrRef>(_gameId);

            // Load data
            AttribSysPack pack = new AttribSysPack();
            Database database = new Database(new DatabaseOptions(_gameId, DatabaseType.X86Database));

            using (FileStream fs = new FileStream(@"data\c4schema.res", FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                pack.Load(br, database);
            }

            using (FileStream fs = new FileStream(@"data\c4indbcontent.res", FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                pack.Load(br, database);
            }

            database.CompleteLoad();
            TypeRegistry.ListUnknownTypes();

            foreach (var typeInfo in database.Types)
            {
                Debug.WriteLine("TYPE: {0} (size {1})", typeInfo.Name, typeInfo.Size);
            }
        }
    }
}
