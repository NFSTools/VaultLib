using CommandLine;
using CoreLibraries.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Pack;
using VaultLib.Core.Types.Abstractions;

namespace VaultCLI
{
    internal static class Program
    {
        private enum OperationMode
        {
            Load,
            LoadBenchmark,
            Save
        }

        private class ProgramArgs
        {
            [Option('g', "game", Required = true, HelpText = "The game that the database is for")]
            public string GameID { get; set; }

            [Option('i', "in", Required = true, HelpText = "The database files to read")]
            public IEnumerable<string> Files { get; set; }

            [Option('o', "out", Required = true, HelpText = "The directory to place generated files in")]
            public string OutputDirectory { get; set; }

            [Option('m', "mode", Default = OperationMode.Load, HelpText = "The mode to run the app in. Defaults to Load.")]
            public OperationMode Mode { get; set; }
        }

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ProgramArgs>(args).WithParsed(RunProgram);
        }

        private static void RunProgram(ProgramArgs args)
        {
            ModuleLoader moduleLoader = new ModuleLoader("VaultLib.Support.*.dll");
            moduleLoader.Load();

            switch (args.Mode)
            {
                case OperationMode.Load:
                    RunLoad(args);
                    break;
                case OperationMode.LoadBenchmark:
                    RunLoadBenchmark(args);
                    break;
                case OperationMode.Save:
                    RunSave(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(args.Mode), "This should never happen...");
            }
        }

        private static void RunLoadBenchmark(ProgramArgs args)
        {
            throw new NotImplementedException();
        }

        private static void RunSave(ProgramArgs args)
        {
            throw new NotImplementedException();
        }

        private static void RunLoad(ProgramArgs args)
        {
            Debug.WriteLine("Loading database");

            HashManager.LoadDictionary("hashes.txt");

            Database database = new Database(new DatabaseOptions(args.GameID, DatabaseType.X86Database));
            Dictionary<string, IList<Vault>> fileDictionary;
            using (new DatabaseLoadingWrapper(database))
            {
                List<string> fileList = new List<string>(args.Files);
                fileDictionary = fileList.ToDictionary(c => c, c => LoadFileToDB(database, c));
            }

            Debug.WriteLine("Loaded database!");
            Debug.WriteLine("Listing unknown types:");
            TypeRegistry.ListUnknownTypes();
            Debug.WriteLine("Listing all types:");
            foreach (DatabaseTypeInfo typeInfo in database.Types.OrderBy(t => t.Name))
            {
                Debug.WriteLine("\t{0} (size {1})", typeInfo.Name, typeInfo.Size);
            }
            Debug.WriteLine("Re-saving files...");
            foreach (var pair in fileDictionary)
            {
                Debug.WriteLine("\tRe-saving {0} ({1} vaults)", pair.Key, pair.Value.Count);

                using FileStream fs = new FileStream(pair.Key + ".gen", FileMode.Create, FileAccess.Write);
                using BinaryWriter bw = new BinaryWriter(fs);

                StandardVaultPack svp = new StandardVaultPack();
                svp.Save(bw, pair.Value);
            }

            // test data API
            //VltCollection collection = database.RowManager.FindCollectionByName("pvehicle", "240sx");
            //int chassisLength = collection.GetListLength("chassis");

            //for (int i = 0; i < chassisLength; i++)
            //{
            //    Debug.WriteLine("chassis[{0}] = {1}", i, collection.GetDataValue<BaseRefSpec>("chassis", i));
            //}
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
