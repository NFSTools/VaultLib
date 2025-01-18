// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:59 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core.Data;
using VaultLib.Core.Exports;
using VaultLib.Core.Hashing;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.DB
{
    /// <summary>
    ///     The <see cref="Database" /> is the powerhouse of the library. It keeps track of all data that is loaded.
    /// </summary>
    public class Database
    {
        private Dictionary<VltCollection, ulong> _parentKeyDictionary = new Dictionary<VltCollection, ulong>();

        /// <summary>
        /// Initializes the database. Sets up data collections.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="exportFactory"></param>
        public Database(DatabaseOptions options, ExportFactory exportFactory)
        {
            Options = options;
            Classes = new List<VltClass>();
            Types = new List<DatabaseTypeInfo>();
            Vaults = new List<Vault>();
            RowManager = new RowManager(this);
            TypeRegistry = new TypeRegistry();
            ExportFactory = exportFactory;
        }

        public DatabaseOptions Options { get; }

        public RowManager RowManager { get; }

        public List<VltClass> Classes { get; }

        public List<DatabaseTypeInfo> Types { get; }

        public TypeRegistry TypeRegistry { get; }

        public ExportFactory ExportFactory { get; }

        public List<Vault> Vaults { get; }

        /// <summary>
        /// Adds a new <see cref="VltClass"/> to the list of classes.
        /// </summary>
        /// <param name="vltClass">The <see cref="VltClass"/> to add to the database.</param>
        public void AddClass(VltClass vltClass)
        {
            Classes.Add(vltClass);
        }

        /// <summary>
        /// Locates and returns the <see cref="VltClass"/> with the given name.
        /// </summary>
        /// <param name="name">The name of the class to search for.</param>
        /// <returns>The <see cref="VltClass"/> with the given name.</returns>
        /// <exception cref="InvalidOperationException">if no class can be found</exception>
        public VltClass FindClass(string name)
        {
            return Classes.First(c => c.Name == name);
        }

        public Vault FindVault(string name)
        {
            return Vaults.First(v => v.Name == name);
        }

        /// <summary>
        ///     Loads data into a <see cref="Vault" /> instance.
        /// </summary>
        /// <param name="vault">The vault to be read and loaded.</param>
        /// <param name="readWrapper">The provider of the vault stream readers.</param>
        public void LoadVault(Vault vault, VaultReadWrapper readWrapper)
        {
            Debug.Assert(vault.Database == null, "vault.Database == null");
            Debug.Assert(vault.BinStream != null, "vault.BinStream != null");
            Debug.Assert(vault.VltStream != null, "vault.VltStream != null");

            vault.Database = this;
            BinaryReader binStreamReader = readWrapper.BinReader;
            BinaryReader vltStreamReader = readWrapper.VltReader;

            ChunkReader binChunkReader = new ChunkReader(binStreamReader);
            ChunkReader vltChunkReader = new ChunkReader(vltStreamReader);

            var vaultLoadContext = new VaultReadContext(vault);

            //Debug.WriteLine("Processing BIN chunks");
            processBinChunks(vaultLoadContext, binChunkReader);

            //Debug.WriteLine("Processing VLT chunks");
            processVltChunks(vaultLoadContext, vltChunkReader);

            //Debug.WriteLine("Processing pointers");
            fixPointers(vault, VltPointerType.Bin, vault.BinStream);
            fixPointers(vault, VltPointerType.Vlt, vault.VltStream);

            //Debug.WriteLine("Reading exports");
            ReadExports(vaultLoadContext, vltStreamReader, binStreamReader);

            Vaults.Add(vault);
        }

        /// <summary>
        ///     Called after all vaults have been loaded in order to generate a proper hierarchy.
        /// </summary>
        public void CompleteLoad()
        {
            ulong Hash(string s)
            {
                return Options.Type == DatabaseType.X64Database ? Vlt64Hasher.Hash(s) : Vlt32Hasher.Hash(s);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            Dictionary<VltClass, ulong> hashDictionary = Classes.ToDictionary(c => c, c => Hash(c.Name));
            Dictionary<ulong, Dictionary<ulong, VltCollection>> collectionDictionary =
                RowManager.Rows.GroupBy(r => hashDictionary[r.Class])
                    .ToDictionary(g => g.Key, g => g.ToDictionary(c => Hash(c.Name), c => c));

            for (int i = RowManager.Rows.Count - 1; i >= 0; i--)
            {
                VltCollection row = RowManager.Rows[i];

                if (_parentKeyDictionary.TryGetValue(row, out ulong parentKey))
                {
                    VltCollection parentCollection = collectionDictionary[hashDictionary[row.Class]][parentKey];
                    parentCollection.AddChild(row);
                    RowManager.Rows.RemoveAt(i);
                }
            }

            stopwatch.Stop();
            _parentKeyDictionary.Clear();
        }

        #region Internal Data Reading

        private void ReadExports(VaultReadContext context, BinaryReader vltStreamReader, BinaryReader binStreamReader)
        {
            foreach (Exports.BaseExport vaultExport in context.Vault.Exports)
            {
                vltStreamReader.BaseStream.Position = vaultExport.Offset;
                vaultExport.Read(context, vltStreamReader);
#if DEBUG
                if ((vltStreamReader.BaseStream.Position - vaultExport.Offset) != vaultExport.Size)
                    throw new Exception();
#endif

                if (vaultExport is IPointerObject pointerObject)
                {
                    pointerObject.ReadPointerData(context, binStreamReader);
                }

                if (vaultExport is BaseCollectionLoad bcl)
                {
                    if (bcl.ParentKey != 0)
                    {
                        _parentKeyDictionary[bcl.Collection] = bcl.ParentKey;
                    }
                }
            }

            context.Vault.IsPrimaryVault = context.Vault.Exports.OfType<BaseClassLoad>().Any();
        }

        private void fixPointers(Vault vault, VltPointerType pointerType, Stream stream)
        {
            IEnumerable<VltPointer> pointers =
                from pointer in vault.Pointers where pointer.Type == pointerType select pointer;

            ByteOrder byteOrder = vault.ByteOrder;
            bool isBigEndian = byteOrder == ByteOrder.Big;

            foreach (VltPointer pointer in pointers)
            {
                stream.Position = pointer.FixUpOffset;
                uint destination = pointer.Destination;
                byte[] destBytes = BitConverter.GetBytes(destination);

                if (isBigEndian)
                {
                    Array.Reverse(destBytes);
                }

                stream.Write(destBytes, 0, 4);
            }
        }

        private void processBinChunks(VaultReadContext context, ChunkReader chunkReader)
        {
            chunkReader.NextChunk().Read(context, chunkReader.Reader);
        }

        private void processVltChunks(VaultReadContext context, ChunkReader chunkReader)
        {
            while (chunkReader.Reader.BaseStream.Position < chunkReader.Reader.BaseStream.Length)
            {
                Chunks.ChunkBase chunk = chunkReader.NextChunk();

                if (chunk == null)
                {
                    break;
                }

                chunk.Read(context, chunkReader.Reader);
                chunk.GoToEnd(context.Vault.VltStream);
            }
        }

        #endregion
    }
}