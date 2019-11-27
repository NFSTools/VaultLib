// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 8:59 PM.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CoreLibraries.Data;
using CoreLibraries.GameUtilities;
using CoreLibraries.IO;
using VaultLib.Core.Chunks;
using VaultLib.Core.Data;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.DB
{
    /// <summary>
    /// The <see cref="Database"/> is the powerhouse of the library. It keeps track of all data that is loaded.
    /// </summary>
    public class Database : IDataEntity
    {
        public RowManager RowManager { get; }

        public List<VLTClass> Classes { get; }

        public List<DatabaseLoadedFile> Files { get; }

        public List<DatabaseTypeInfo> Types { get; }

        public string Game { get; }

        public List<VLTCollection> Rows { get; }

        public bool Is64Bit { get; set; }

        public Database(string game)
        {
            this.Rows = new List<VLTCollection>();
            this.RowManager = new RowManager(this);
            this.Game = game;
            this.Classes = new List<VLTClass>();
            this.Files = new List<DatabaseLoadedFile>();
            this.Types = new List<DatabaseTypeInfo>();
        }

        public void AddClass(VLTClass vltClass)
        {
            this.Classes.Add(vltClass);
        }

        public VLTClass FindClass(string name) => this.Classes.Find(c => c.Name == name);
        public VLTClass FindClass(ulong hash) => this.Classes.Find(c => c.NameHash == hash);

        /// <summary>
        /// Loads a vault
        /// </summary>
        /// <param name="file">The <see cref="DatabaseLoadedFile"/> instance to register the vault with.</param>
        /// <param name="vault">The vault to be read and loaded.</param>
        /// <param name="loadingWrapper">The provider of the vault stream readers.</param>
        public void LoadVault(DatabaseLoadedFile file, Vault vault, VaultLoadingWrapper loadingWrapper)
        {
            Debug.Assert(vault.Database == null, "vault.Database == null");
            Debug.Assert(vault.BinStream != null, "vault.BinStream != null");
            Debug.Assert(vault.VltStream != null, "vault.VltStream != null");

            vault.Database = this;
            var binStreamReader = loadingWrapper.BinReader;
            var vltStreamReader = loadingWrapper.VltReader;

            var binChunkReader = new ChunkReader(binStreamReader);
            var vltChunkReader = new ChunkReader(vltStreamReader);

            Debug.WriteLine("Processing BIN chunks");
            this.processBinChunks(vault, binChunkReader);

            Debug.WriteLine("Processing VLT chunks");
            this.processVltChunks(vault, vltChunkReader);

            Debug.WriteLine("Processing pointers");
            this.fixPointers(vault, VLTPointerType.Bin, vault.BinStream);
            this.fixPointers(vault, VLTPointerType.Vlt, vault.VltStream);

            Debug.WriteLine("Reading exports");
            this.readExports(vault, vltStreamReader, binStreamReader);

            file.Vaults.Add(vault);
        }

        public void LoadFile(DatabaseLoadedFile file) => Files.Add(file);

        public void AddRow(VLTCollection collection)
        {
            Rows.Add(collection);
        }

        /// <summary>
        /// Called after all files have been loaded in order to generate a proper hierarchy
        /// </summary>
        public void CompleteLoad()
        {
            Debug.WriteLine("Resolving hierarchy for {0} rows", Rows.Count);

            Stopwatch stopwatch = Stopwatch.StartNew();

            Dictionary<ulong, Dictionary<ulong, VLTCollection>> collectionDictionary =
                Rows.GroupBy(r => r.Class.NameHash).ToDictionary(g => g.Key, g => g.ToDictionary(c => c.Key, c => c));

            for (int i = Rows.Count - 1; i >= 0; i--)
            {
                VLTCollection row = Rows[i];

                if (row.ParentKey != 0)
                {
                    VLTCollection parentCollection = collectionDictionary[row.Class.NameHash][row.ParentKey];
                    row.SetParent(parentCollection);

                    Rows.RemoveAt(i);
                }
            }

            stopwatch.Stop();

            Debug.WriteLine("Resolved hierarchy in {0}ms, now we have {1} rows", stopwatch.ElapsedMilliseconds, Rows.Count);
        }

        /// <summary>
        /// Returns either a 32-bit or 64-bit Jenkins hash of the given text.
        /// 64-bit hashes are returned if this database is representing 64-bit VLT data.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ulong AutoExportKey(string name)
        {
            return Is64Bit ? VLT64Hasher.Hash(name) : VLT32Hasher.Hash(name);
        }

        #region Internal Data Reading

        private void readExports(Vault vault, BinaryReader vltStreamReader, BinaryReader binStreamReader)
        {
            foreach (var vaultExport in vault.Exports)
            {
                vltStreamReader.BaseStream.Position = vaultExport.Offset;
                vaultExport.Read(vault, vltStreamReader);

                if (vaultExport is IPointerObject pointerObject)
                {
                    pointerObject.ReadPointerData(vault, binStreamReader);
                }
            }
        }

        private void fixPointers(Vault vault, VLTPointerType pointerType, Stream stream)
        {
            IEnumerable<VLTPointer> pointers =
                from pointer in vault.Pointers where pointer.Type == pointerType select pointer;

            ByteOrder byteOrder = vault.ByteOrder;

            foreach (var pointer in pointers)
            {
                stream.Position = pointer.FixUpOffset;
                uint destination = pointer.Destination;
                byte[] destBytes = byteOrder == ByteOrder.Little ? new[]{
                    (byte)(destination & 0xff),
                    (byte)((destination >> 8) & 0xff),
                    (byte)((destination >> 16) & 0xff),
                    (byte)((destination >> 24) & 0xff)
                } : new[]{
                    (byte)((destination >> 24) & 0xff),
                    (byte)((destination >> 16) & 0xff),
                    (byte)((destination >> 8) & 0xff),
                    (byte)(destination & 0xff),
                };

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
                ChunkBase chunk = chunkReader.NextChunk(vault);

                if (chunk == null)
                {
                    break;
                }

                chunk.Read(vault, chunkReader.Reader);
                chunk.GoToEnd(vault.VltStream);
            }
        }

        #endregion

        public ICollection<IDataEntity> GetChildren()
        {
            return new List<IDataEntity>();
        }

        public void AddChild(IDataEntity child)
        {
            throw new System.NotImplementedException();
        }

        public string TypeName => "AttribSysDB";
        public string Name => "VLT Database";
    }
}