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
using VaultLib.Core.IO;
using VaultLib.Core.Utils;
using VLT64Hasher = VaultLib.Core.Hashing.VLT64Hasher;

namespace VaultLib.Core.DB
{
    /// <summary>
    ///     The <see cref="Database" /> is the powerhouse of the library. It keeps track of all data that is loaded.
    /// </summary>
    public class Database
    {
        private Dictionary<VltCollection, string> _parentKeyDictionary = new Dictionary<VltCollection, string>();

        /// <summary>
        /// Initializes the database. Sets up data collections.
        /// </summary>
        /// <param name="options"></param>
        public Database(DatabaseOptions options)
        {
            Options = options;
            Classes = new List<VltClass>();
            Types = new List<DatabaseTypeInfo>();
            RowManager = new RowManager(this);
        }

        public DatabaseOptions Options { get; }

        public RowManager RowManager { get; }

        public List<VltClass> Classes { get; }

        public List<DatabaseTypeInfo> Types { get; }

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

        /// <summary>
        ///     Loads data into a <see cref="Vault" /> instance.
        /// </summary>
        /// <param name="vault">The vault to be read and loaded.</param>
        /// <param name="loadingWrapper">The provider of the vault stream readers.</param>
        public void LoadVault(Vault vault, VaultLoadingWrapper loadingWrapper)
        {
            Debug.Assert(vault.Database == null, "vault.Database == null");
            Debug.Assert(vault.BinStream != null, "vault.BinStream != null");
            Debug.Assert(vault.VltStream != null, "vault.VltStream != null");

            vault.Database = this;
            BinaryReader binStreamReader = loadingWrapper.BinReader;
            BinaryReader vltStreamReader = loadingWrapper.VltReader;

            ChunkReader binChunkReader = new ChunkReader(binStreamReader);
            ChunkReader vltChunkReader = new ChunkReader(vltStreamReader);

            //Debug.WriteLine("Processing BIN chunks");
            processBinChunks(vault, binChunkReader);

            //Debug.WriteLine("Processing VLT chunks");
            processVltChunks(vault, vltChunkReader);

            //Debug.WriteLine("Processing pointers");
            fixPointers(vault, VltPointerType.Bin, vault.BinStream);
            fixPointers(vault, VltPointerType.Vlt, vault.VltStream);

            //Debug.WriteLine("Reading exports");
            ReadExports(vault, vltStreamReader, binStreamReader);
        }

        /// <summary>
        ///     Called after all vaults have been loaded in order to generate a proper hierarchy.
        /// </summary>
        public void CompleteLoad()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Dictionary<VltClass, ulong> hashDictionary = Classes.ToDictionary(c => c, c => VLT64Hasher.Hash(c.Name));
            Dictionary<ulong, Dictionary<string, VltCollection>> collectionDictionary =
                RowManager.Rows.GroupBy(r => hashDictionary[r.Class])
                    .ToDictionary(g => g.Key, g => g.ToDictionary(c => c.Name, c => c));

            for (int i = RowManager.Rows.Count - 1; i >= 0; i--)
            {
                VltCollection row = RowManager.Rows[i];

                if (_parentKeyDictionary.TryGetValue(row, out string parentKey))
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

        private void ReadExports(Vault vault, BinaryReader vltStreamReader, BinaryReader binStreamReader)
        {
            foreach (Exports.BaseExport vaultExport in vault.Exports)
            {
                vltStreamReader.BaseStream.Position = vaultExport.Offset;
                vaultExport.Read(vault, vltStreamReader);
#if DEBUG
                if ((vltStreamReader.BaseStream.Position - vaultExport.Offset) != vaultExport.Size)
                    throw new Exception();
#endif

                if (vaultExport is IPointerObject pointerObject)
                {
                    pointerObject.ReadPointerData(vault, binStreamReader);
                }

                if (vaultExport is BaseCollectionLoad bcl)
                {
                    string parentKey = bcl.ParentKey;
                    if (!string.IsNullOrEmpty(parentKey))
                    {
                        _parentKeyDictionary[bcl.Collection] = parentKey;
                    }
                }
            }

            vault.IsPrimaryVault = vault.Exports.OfType<BaseClassLoad>().Any();
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

        private void processBinChunks(Vault vault, ChunkReader chunkReader)
        {
            chunkReader.NextChunk(vault).Read(vault, chunkReader.Reader);
        }

        private void processVltChunks(Vault vault, ChunkReader chunkReader)
        {
            while (chunkReader.Reader.BaseStream.Position < chunkReader.Reader.BaseStream.Length)
            {
                Chunks.ChunkBase chunk = chunkReader.NextChunk(vault);

                if (chunk == null)
                {
                    break;
                }

                chunk.Read(vault, chunkReader.Reader);
                chunk.GoToEnd(vault.VltStream);
            }
        }

        #endregion
    }
}