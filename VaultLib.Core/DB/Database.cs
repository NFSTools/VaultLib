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
using VaultLib.Core.Types.Attrib.Query;
using VaultLib.Core.Utils;

namespace VaultLib.Core.DB;

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

    public Vault LoadVault(VaultReadWrapper readWrapper)
    {
        var vault = new Vault(readWrapper.VaultName)
        {
            Database = this,
            ByteOrder = readWrapper.ByteOrder
        };

        var binStreamReader = CreateStreamReader(readWrapper.BinStream, readWrapper.ByteOrder);
        var vltStreamReader = CreateStreamReader(readWrapper.VltStream, readWrapper.ByteOrder);

        Debug.WriteLine("[IN] vault {0}: bin size 0x{1:X} vlt size 0x{2:X}", vault.Name, readWrapper.BinStream.Length,
            readWrapper.VltStream.Length);

        var binChunkReader = new ChunkReader(binStreamReader);
        var vltChunkReader = new ChunkReader(vltStreamReader);

        var vaultLoadContext = new VaultReadContext(vault, readWrapper.BinStream, readWrapper.VltStream);

        //Debug.WriteLine("Processing BIN chunks");
        processBinChunks(vaultLoadContext, binChunkReader);

        //Debug.WriteLine("Processing VLT chunks");
        processVltChunks(vaultLoadContext, vltChunkReader);

        //Debug.WriteLine("Processing pointers");
        fixPointers(vaultLoadContext, VltPointerType.Bin, readWrapper.BinStream);
        fixPointers(vaultLoadContext, VltPointerType.Vlt, readWrapper.VltStream);

        //Debug.WriteLine("Reading exports");
        ReadExports(vaultLoadContext, vltStreamReader, binStreamReader);

        Vaults.Add(vault);

        return vault;
    }

    private static BinaryReader CreateStreamReader(Stream stream, ByteOrder byteOrder)
    {
        return byteOrder == ByteOrder.Big ? new BigEndianBinaryReader(stream) : new BinaryReader(stream);
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

        var stopwatch = Stopwatch.StartNew();

        var classToCollections = new Dictionary<VltClass, Dictionary<ulong, VltCollection>>();

        foreach (var vltCollection in RowManager.Rows)
        {
            if (!classToCollections.TryGetValue(vltCollection.Class, out var collections))
            {
                collections = new Dictionary<ulong, VltCollection>();
                classToCollections.Add(vltCollection.Class, collections);
            }

            var hash = Hash(vltCollection.Name);
            if (!collections.TryAdd(hash, vltCollection))
            {
                Debug.WriteLine("WARN: duplicate key detected in class {2}: {0} (0x{1:X})", vltCollection.Name, hash,
                    vltCollection.Class.Name);
            }
        }

        foreach (var vltCollection in RowManager.Rows)
        {
            if (!_parentKeyDictionary.TryGetValue(vltCollection, out var parentKey)) continue;
            
            var collections = classToCollections[vltCollection.Class];

            if (!collections.TryGetValue(parentKey, out var parentCollection))
            {
                throw new KeyNotFoundException(
                    $"could not find parent collection for {vltCollection.Name}: 0x{parentKey:X}");
            }

            parentCollection.AddChild(vltCollection);
        }

        stopwatch.Stop();
        _parentKeyDictionary.Clear();

        FixupStaticData();
    }

    private void FixupStaticData()
    {
        foreach (var vltClass in Classes)
        {
            foreach (var staticField in vltClass.StaticFields)
            {
                // TODO: We should really have some kind of post-processing abstraction for static data.
                if (staticField.StaticValue is Static_Inorder_N_to_1 staticTree)
                {
                    Static_Inorder_N_to_1.TreeNodeType? nodeType = null;
                    for (var i = 0; i < staticTree.Keys.Count; i++)
                    {
                        var key = staticTree.Keys[i];
                        var indexTableEntry = staticTree.Indices[i];
                        var values = staticTree.Values.GetRange(indexTableEntry.Index, indexTableEntry.Count);

                        var keyToName = HashManager.ResolveVlt(key);

                        if (key != 0)
                        {
                            var collection = RowManager.FindCollectionByName(vltClass.Name, keyToName);

                            if (collection == null)
                            {
                                throw new InvalidDataException(
                                    $"static index references nonexistent collection: {keyToName}");
                            }

                            if (values.Count == 1)
                            {
                                var linkedKey = values[0];
                                var linkedKeyToName = HashManager.ResolveVlt(linkedKey);
                                var linkedCollection = RowManager.FindCollectionByName(vltClass.Name, linkedKeyToName);

                                if (ReferenceEquals(collection.Parent, linkedCollection))
                                {
                                    if (nodeType == null)
                                    {
                                        nodeType = Static_Inorder_N_to_1.TreeNodeType.ParentKey;
                                    }
                                    else if (nodeType != Static_Inorder_N_to_1.TreeNodeType.ParentKey)
                                    {
                                        throw new Exception("strange mixture of nodes in static index");
                                    }
                                }
                                else
                                {
                                    nodeType = Static_Inorder_N_to_1.TreeNodeType.ChildKeys;
                                }
                            }
                            else if (nodeType == Static_Inorder_N_to_1.TreeNodeType.ParentKey)
                            {
                                throw new Exception("each node in a ParentKey index must have exactly 1 value");
                            }
                            else
                            {
                                nodeType = Static_Inorder_N_to_1.TreeNodeType.ChildKeys;
                            }
                        }
                    }

                    staticTree.NodeType = nodeType ?? Static_Inorder_N_to_1.TreeNodeType.ChildKeys;
                }
            }
        }
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

    private void fixPointers(VaultReadContext context, VltPointerType pointerType, Stream stream)
    {
        IEnumerable<VltPointer> pointers =
            from pointer in context.Pointers where pointer.Type == pointerType select pointer;

        ByteOrder byteOrder = context.Vault.ByteOrder;
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
            chunk.GoToEnd(chunkReader.Reader.BaseStream);
        }
    }

    #endregion
}