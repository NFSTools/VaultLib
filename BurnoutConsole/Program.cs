using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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
            List<string> fileList = new List<string> { "schema.bin", "F1_02_EX.bin", "CameraVault.bin", "XUSBHB1_AttribSys.bin", "PUSMC01_AttribSys.bin" };
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

            if (Directory.Exists("patches"))
            {
                Debug.WriteLine("applying patches");

                foreach (var file in Directory.GetFiles("patches", "*.patch"))
                {
                    Debug.WriteLine("applying {0}", new object[] { file });
                    ApplyPatchToDatabase(file, database);
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

        private static void ApplyPatchToDatabase(string file, Database database)
        {
            VltCollection collection = null;

            foreach (var line in File.ReadAllLines(file))
            {
                if (line.StartsWith('#')) continue;

                string[] parts = line.Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
                if (parts.Length < 2)
                {
                    throw new Exception("could not process: " + line);
                }

                switch (parts[0])
                {
                    case "collection":
                        collection = ProcessCollectionPatchLine(parts, database);
                        break;
                    case "set":
                        HandleSetPatchLine(parts, collection, database);
                        break;
                    default:
                        throw new Exception("unknown command: " + parts[0]);
                }
            }
        }

        private static void HandleSetPatchLine(string[] parts, VltCollection collection, Database database)
        {
            // set  YawDampingOnTakeOff [prop] 10
            if (collection == null)
            {
                throw new Exception("no collection to operate on");
            }

            if (parts.Length > 4)
            {
                throw new Exception("expected maximum of 3 arguments, got " + (parts.Length - 1));
            }

            string fieldName = parts[1];
            if (!collection.Class.TryGetField(fieldName, out VltClassField field))
            {
                throw new Exception("unknown field: " + fieldName);
            }

            string prop = parts.Length == 4 ? parts[2] : string.Empty;
            string valueText = parts[parts.Length == 4 ? 3 : 2];

            object value = field.TypeName switch
            {
                "EA::Reflection::Int8" => sbyte.Parse(valueText),
                "EA::Reflection::UInt8" => byte.Parse(valueText),
                "EA::Reflection::Int16" => short.Parse(valueText),
                "EA::Reflection::UInt16" => ushort.Parse(valueText),
                "EA::Reflection::Int32" => int.Parse(valueText),
                "EA::Reflection::UInt32" => uint.Parse(valueText),
                "EA::Reflection::Float" => float.Parse(valueText),
                "EA::Reflection::Text" => valueText.Replace("\\t", "\t"),
                _ => throw new Exception("Cannot handle: " + field.TypeName)
            };

            if (field.IsArray)
            {
                if (string.IsNullOrEmpty(prop))
                {
                    throw new Exception("array index required");
                }

                if (int.TryParse(prop, out int index))
                {
                    collection.SetDataValue(fieldName, index, value);
                }
                else
                {
                    throw new Exception("could not parse index");
                }
            }
            else
            {
                collection.SetDataValue(fieldName, value);
            }
        }

        private static VltCollection ProcessCollectionPatchLine(string[] parts, Database database)
        {
            // collection   physicsvehiclebaseattribs   0xD72AF68A8CECC505

            if (parts.Length != 3)
            {
                throw new Exception("expected 2 arguments, got " + (parts.Length - 1));
            }

            string className = parts[1];
            string collectionName = parts[2];

            VltCollection collection = database.RowManager.FindCollectionByName(className, collectionName);

            if (collection == null)
            {
                throw new Exception("could not find collection: " + collectionName + " in class: " + className);
            }

            return collection;
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
