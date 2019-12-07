// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:59 PM.

using CoreLibraries.GameUtilities;
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

namespace VaultLib.Core.DB
{
    /// <summary>
    ///     The <see cref="Database" /> is the powerhouse of the library. It keeps track of all data that is loaded.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Initializes the database. Sets up data collections.
        /// </summary>
        /// <param name="options"></param>
        public Database(DatabaseOptions options)
        {
            Options = options;
            Classes = new List<VLTClass>();
            Types = new List<DatabaseTypeInfo>();
            Files = new List<DatabaseLoadedFile>();
            RowManager = new RowManager(this);
        }

        public DatabaseOptions Options { get; }

        public RowManager RowManager { get; }

        public List<VLTClass> Classes { get; }

        public List<DatabaseLoadedFile> Files { get; }

        public List<DatabaseTypeInfo> Types { get; }

        /// <summary>
        /// Adds a new <see cref="VLTClass"/> to the list of classes.
        /// </summary>
        /// <param name="vltClass">The <see cref="VLTClass"/> to add to the database.</param>
        public void AddClass(VLTClass vltClass)
        {
            Classes.Add(vltClass);
        }

        /// <summary>
        /// Locates and returns the <see cref="VLTClass"/> with the given name.
        /// </summary>
        /// <param name="name">The name of the class to search for.</param>
        /// <returns>The <see cref="VLTClass"/> with the given name.</returns>
        /// <exception cref="InvalidOperationException">if no class can be found</exception>
        public VLTClass FindClass(string name)
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

            Debug.WriteLine("Processing BIN chunks");
            processBinChunks(vault, binChunkReader);

            Debug.WriteLine("Processing VLT chunks");
            processVltChunks(vault, vltChunkReader);

            Debug.WriteLine("Processing pointers");
            fixPointers(vault, VLTPointerType.Bin, vault.BinStream);
            fixPointers(vault, VLTPointerType.Vlt, vault.VltStream);

            Debug.WriteLine("Reading exports");
            readExports(vault, vltStreamReader, binStreamReader);
        }

        /// <summary>
        ///     Called after all files have been loaded in order to generate a proper hierarchy
        /// </summary>
        public void CompleteLoad()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Dictionary<VLTClass, ulong> hashDictionary = Classes.ToDictionary(c => c, c => VLT64Hasher.Hash(c.Name));
            Dictionary<ulong, Dictionary<ulong, VLTCollection>> collectionDictionary =
                RowManager.Rows.GroupBy(r => hashDictionary[r.Class])
                    .ToDictionary(g => g.Key, g => g.ToDictionary(c => c.Key, c => c));

            for (int i = RowManager.Rows.Count - 1; i >= 0; i--)
            {
                VLTCollection row = RowManager.Rows[i];

                if (row.ParentKey != 0)
                {
                    VLTCollection parentCollection = collectionDictionary[hashDictionary[row.Class]][row.ParentKey];
                    row.SetParent(parentCollection);

                    RowManager.Rows.RemoveAt(i);
                }
            }

            stopwatch.Stop();
        }

        #region Internal Data Reading

        private void readExports(Vault vault, BinaryReader vltStreamReader, BinaryReader binStreamReader)
        {
            foreach (Exports.BaseExport vaultExport in vault.Exports)
            {
                vltStreamReader.BaseStream.Position = vaultExport.Offset;
                vaultExport.Read(vault, vltStreamReader);

                if (vaultExport is IPointerObject pointerObject)
                {
                    pointerObject.ReadPointerData(vault, binStreamReader);
                }
            }

            vault.IsPrimaryVault = vault.Exports.OfType<BaseClassLoad>().Any();
        }

        private void fixPointers(Vault vault, VLTPointerType pointerType, Stream stream)
        {
            IEnumerable<VLTPointer> pointers =
                from pointer in vault.Pointers where pointer.Type == pointerType select pointer;

            ByteOrder byteOrder = vault.ByteOrder;
            bool isBigEndian = byteOrder == ByteOrder.Big;

            foreach (VLTPointer pointer in pointers)
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