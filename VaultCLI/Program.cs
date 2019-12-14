using CommandLine;
using CoreLibraries.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Pack;

namespace VaultCLI
{
    internal static class Program
    {
        private enum OperationMode
        {
            Load,
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
                case OperationMode.Save:
                    RunSave(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(args.Mode), "This should never happen...");
            }
        }

        private static void RunSave(ProgramArgs args)
        {
            throw new NotImplementedException();
        }

        private static void RunLoad(ProgramArgs args)
        {
            Debug.WriteLine("Loading database");

            HashManager.LoadDictionary("hashes.txt");
            Stopwatch stopwatch = Stopwatch.StartNew();

            int numIterations = 50;

            for (int i = 0; i < numIterations; i++)
            {
                Database database = new Database(new DatabaseOptions(args.GameID, DatabaseType.X86Database));
                using (new DatabaseLoadingWrapper(database))
                {
                    foreach (string file in args.Files)
                    {
                        LoadFileToDB(database, file);
                        //fileDictionary[file] = LoadFileToDB(database, file);
                    }
                }
            }

            //Dictionary<string, IList<Vault>> fileDictionary = new Dictionary<string, IList<Vault>>();




            //database.CompleteLoad();
            stopwatch.Stop();

            Console.WriteLine("Test done in {0}ms (avg {1}ms)", stopwatch.ElapsedMilliseconds, ((float)stopwatch.ElapsedMilliseconds) / numIterations);

            //DisplayHierarchy(database);

            TypeRegistry.ListUnknownTypes();

            //Debug.WriteLine("generating code");
            //string codeGenDirectory = Path.Combine("gen-code", args.GameID);
            //Directory.CreateDirectory(codeGenDirectory);

            //CSharpCodeGenerator cscg = new CSharpCodeGenerator(database);

            //foreach (VaultLib.Core.Data.VltClass databaseClass in database.Classes)
            //{
            //    cscg.WriteCodeToFile(databaseClass, codeGenDirectory);
            //}

            //stopwatch = Stopwatch.StartNew();
            //Debug.WriteLine("re-saving");

            //foreach (var (path, vaults) in fileDictionary)
            //{
            //    using BinaryWriter bw = new BinaryWriter(File.Open(path + ".gen", FileMode.Create));
            //    StandardVaultPack vaultPack = new StandardVaultPack();
            //    vaultPack.Save(bw, vaults);
            //}
            //stopwatch.Stop();
            //Debug.WriteLine("re-saved in {0}ms", stopwatch.ElapsedMilliseconds);
        }

        private static void DisplayHierarchy(Database database)
        {
            foreach (var databaseClass in database.Classes)
            {
                Debug.WriteLine("{0} ({1} fields)", databaseClass.Name, databaseClass.Fields.Count);

                DisplayCollections(databaseClass, database.RowManager.EnumerateCollections(databaseClass.Name));
            }
        }

        private static void DisplayCollections(VltClass databaseClass, IEnumerable<VltCollection> collections, int level = 0)
        {
            string indent = new string('\t', level + 1);

            foreach (var collection in collections)
            {
                Debug.WriteLine("{0}{1}", indent, collection.Name);

                if (collection.Children.Count > 0)
                    DisplayCollections(databaseClass, collection.Children, level + 1);
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
