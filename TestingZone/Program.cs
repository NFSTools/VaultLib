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
using VaultLib.Core.Utils;
using VaultLib.Support.World;

namespace TestingZone
{
    class Program
    {
        static void Main(string[] args)
        {
            HashManager.LoadDictionary("hashes.txt");
            new ModuleDef().Load();

            var (db1, fd1) = LoadDatabase(@"nfsw-2015\attributes.bin", @"nfsw-2015\commerce.bin", @"nfsw-2015\fe_attrib.bin");
            var (db2, fd2) = LoadDatabase(@"nfsw-2013\attributes.bin", @"nfsw-2013\commerce.bin", @"nfsw-2013\fe_attrib.bin");

            Vault dbVault1 = db1.FindVault("db");
            Vault dbVault2 = db2.FindVault("db");

            CopyCarData(db2, "bmwm3", dbVault2, db1, dbVault1);
            CopyCarData(db2, "car1070", dbVault2, db1, dbVault1);
            CopyCarData(db2, "car1072", dbVault2, db1, dbVault1);

            Directory.CreateDirectory("generated-files");
            foreach (var pair in fd1)
            {
                Debug.WriteLine("\tRe-saving {0} ({1} vaults)", pair.Key, pair.Value.Count);

                using FileStream fs = new FileStream(Path.Combine("generated-files", Path.GetFileName(pair.Key)), FileMode.Create, FileAccess.Write);
                using BinaryWriter bw = new BinaryWriter(fs);

                StandardVaultPack svp = new StandardVaultPack();
                svp.Save(bw, pair.Value);
            }
        }

        private static void CopyCarData(Database db2, string carName, Vault dbVault2, Database db1, Vault dbVault1)
        {
            VltCollection findVehicleInfo = db2.RowManager.FindCollectionByName("pvehicle", carName);

            if (findVehicleInfo == null)
            {
                throw new Exception("cannot find pvehicle");
            }

            VltCollection findECar = db2.RowManager.FindCollectionByName("ecar", carName);

            if (findECar == null)
            {
                throw new Exception("cannot find ecar");
            }

            List<VltCollection> referencingCollections = new List<VltCollection> {findVehicleInfo, findECar};
            ISet<KeyValuePair<string, string>> referencedCollections = new HashSet<KeyValuePair<string, string>>();

            foreach (var collection in referencingCollections)
            {
                foreach (var collectionReferencer in collection.GetData().Values.OfType<IReferencesCollections>())
                {
                    foreach (var collectionReferenceInfo in collectionReferencer.GetReferencedCollections(db2, dbVault2))
                    {
                        if (collectionReferenceInfo.Destination == null) continue;
                        referencedCollections.Add(new KeyValuePair<string, string>(
                            collectionReferenceInfo.Destination.Class.Name, collectionReferenceInfo.Destination.Name));
                    }
                }
            }

            List<VltCollection> collectionsToAdd = new List<VltCollection>();
            collectionsToAdd.Add(findVehicleInfo);
            collectionsToAdd.Add(findECar);

            foreach (var referencedCollection in referencedCollections)
            {
                Debug.WriteLine("REFERENCE: {0}/{1}", referencedCollection.Key, referencedCollection.Value);

                if (db1.RowManager.FindCollectionByName(referencedCollection.Key, referencedCollection.Value) != null)
                {
                    Debug.WriteLine("\tskipping duplicate reference");
                    continue;
                }

                VltCollection collection =
                    db2.RowManager.FindCollectionByName(referencedCollection.Key, referencedCollection.Value);
                if (collection == null)
                {
                    Debug.WriteLine("\tskipping unknown reference");
                    continue;
                }


                collectionsToAdd.Add(collection);
            }

            foreach (var vltCollection in collectionsToAdd)
            {
                if (db1.RowManager.FindCollectionByName(vltCollection.Class.Name, vltCollection.Name) != null)
                {
                    Debug.WriteLine("\tWon't add {0} because we already have it in the DB", new object[] { vltCollection.ShortPath  });
                    continue;
                }

                VltCollection parentCollection = vltCollection.Parent;
                vltCollection.SetVault(dbVault1);
                vltCollection.Parent?.RemoveChild(vltCollection);

                if (parentCollection != null)
                {
                    parentCollection =
                        db1.RowManager.FindCollectionByName(parentCollection.Class.Name, parentCollection.Name);
                    parentCollection?.AddChild(vltCollection);
                }
                else
                {
                    db1.RowManager.AddCollection(vltCollection);
                }
            }
        }

        private static (Database, Dictionary<string, IList<Vault>>) LoadDatabase(params string[] files)
        {
            Database database = new Database(new DatabaseOptions("WORLD", DatabaseType.X86Database));
            Dictionary<string, IList<Vault>> fileDictionary;
            using (new DatabaseLoadingWrapper(database))
            {
                List<string> fileList = new List<string>(files);
                fileDictionary = fileList.ToDictionary(c => c, c => LoadFileToDB(database, c));
            }

            return (database, fileDictionary);
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
