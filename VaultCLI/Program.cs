using CommandLine;
using CoreLibraries.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.DB;
using VaultLib.Core.Hashing;
using VaultLib.Core.Loading;

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
            Database database = new Database(args.GameID);
            Stopwatch stopwatch = Stopwatch.StartNew();

            foreach (var file in args.Files)
            {
                LoadFileToDB(database, file);
            }

            database.CompleteLoad();
            stopwatch.Stop();

            Debug.WriteLine("Loaded in {0}ms", stopwatch.ElapsedMilliseconds);
            TypeRegistry.ListUnknownTypes();

            var codeGenDirectory = Path.Combine("gen-code", database.Game);
            Directory.CreateDirectory(codeGenDirectory);

            foreach (var databaseClass in database.Classes)
            {
                VaultClassToCodeConverter ctc = new VaultClassToCodeConverter(database, databaseClass);
                ctc.WriteCodeToFile(codeGenDirectory);
            }
        }

        private static void LoadFileToDB(Database database, string file)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(file)))
            {
                StandardVaultPack vaultPack = new StandardVaultPack();
                vaultPack.Load(br, database, new PackLoadingOptions());
            }
        }
    }
}
