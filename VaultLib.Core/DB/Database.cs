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
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Exports;
using VaultLib.Core.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.DB;

/// <summary>
///     The <see cref="Database{TKey}" /> is the powerhouse of the library. It keeps track of all data that is loaded.
/// </summary>
public class Database<TKey> where TKey : struct, IKey<TKey>
{
    private Dictionary<VltCollection<TKey>, TKey> _parentKeyDictionary = new();

    private ByteOrder _expectedByteOrder;

    /// <summary>
    /// Initializes the database. Sets up data collections.
    /// </summary>
    /// <param name="exportFactory"></param>
    /// <param name="typeRegistryBuilder"></param>
    public Database(ExportFactory<TKey> exportFactory, TypeRegistryBuilder<TKey> typeRegistryBuilder)
    {
        Classes = new List<VltClass<TKey>>();
        Types = new List<DatabaseTypeInfo>();
        Vaults = new List<Vault<TKey>>();
        RowManager = new RowManager<TKey>(this);
        TypeRegistry = typeRegistryBuilder.Build(this);
        ExportFactory = exportFactory;
        _expectedByteOrder = typeRegistryBuilder.ByteOrder;
    }

    public RowManager<TKey> RowManager { get; }

    public List<VltClass<TKey>> Classes { get; }

    public List<DatabaseTypeInfo> Types { get; }

    public TypeRegistry<TKey> TypeRegistry { get; }

    public ExportFactory<TKey> ExportFactory { get; }

    public List<Vault<TKey>> Vaults { get; }

    /// <summary>
    /// Adds a new class to the database.
    /// </summary>
    /// <param name="vltClass">The class to add.</param>
    public void AddClass(VltClass<TKey> vltClass)
    {
        Classes.Add(vltClass);
    }

    /// <summary>
    /// Locates the class with a particular key.
    /// </summary>
    /// <param name="key">The key to search for.</param>
    /// <returns>The class with the given key.</returns>
    /// <exception cref="InvalidOperationException">if no class can be found</exception>
    public VltClass<TKey> FindClass(TKey key)
    {
        return Classes.First(c => c.Key == key);
    }

    /// <summary>
    /// Locates the class with a particular name.
    /// </summary>
    /// <param name="name">The name of the class to search for.</param>
    /// <returns>The class with the given name.</returns>
    /// <exception cref="InvalidOperationException">if no class can be found</exception>
    public VltClass<TKey> FindClass(string name)
    {
        return FindClass(TKey.FromString(name));
    }

    public Vault<TKey> FindVault(string name)
    {
        return Vaults.First(v => v.Name == name);
    }

    public Vault<TKey> LoadVault(VaultReadWrapper readWrapper)
    {
        if (_expectedByteOrder != readWrapper.ByteOrder)
        {
            throw new Exception(
                $"Cannot load vault because its byte order ({readWrapper.ByteOrder}) does not match the database's byte order ({_expectedByteOrder}).");
        }

        var vault = new Vault<TKey>(this, readWrapper.VaultName);
        var binStreamReader = CreateStreamReader(readWrapper.BinStream, readWrapper.ByteOrder);
        var vltStreamReader = CreateStreamReader(readWrapper.VltStream, readWrapper.ByteOrder);

        Debug.WriteLine("[IN] vault {0}: bin size 0x{1:X} vlt size 0x{2:X}", vault.Name, readWrapper.BinStream.Length,
            readWrapper.VltStream.Length);

        var binChunkReader = new ChunkReader<TKey>(binStreamReader);
        var vltChunkReader = new ChunkReader<TKey>(vltStreamReader);

        var vaultLoadContext =
            new VaultReadContext<TKey>(vault, readWrapper.BinStream, readWrapper.VltStream, readWrapper.ByteOrder);

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
        var stopwatch = Stopwatch.StartNew();

        var classToCollections = new Dictionary<VltClass<TKey>, Dictionary<TKey, VltCollection<TKey>>>();

        foreach (var vltCollection in RowManager.Rows)
        {
            if (!classToCollections.TryGetValue(vltCollection.Class, out var collections))
            {
                collections = new Dictionary<TKey, VltCollection<TKey>>();
                classToCollections.Add(vltCollection.Class, collections);
            }

            // var hash = Hash(vltCollection.Name);
            if (!collections.TryAdd(vltCollection.Key, vltCollection))
            {
                Debug.WriteLine("WARN: duplicate key detected in class {2}: {0} (0x{1:X})", vltCollection.Key,
                    vltCollection.Key,
                    vltCollection.Class.Key);
            }
        }

        foreach (var vltCollection in RowManager.Rows)
        {
            if (!_parentKeyDictionary.TryGetValue(vltCollection, out var parentKey)) continue;

            var collections = classToCollections[vltCollection.Class];

            if (!collections.TryGetValue(parentKey, out var parentCollection))
            {
                throw new Exception(
                    $"could not find parent collection for {vltCollection.Key}: {parentKey}");
            }

            vltCollection.SetParent(parentCollection);
        }

        stopwatch.Stop();
        _parentKeyDictionary.Clear();

        FixupStaticData();
    }

    private void FixupStaticData()
    {
        // foreach (var vltClass in Classes)
        // {
        //     foreach (var staticField in vltClass.StaticFields)
        //     {
        //         // TODO: We should really have some kind of post-processing abstraction for static data.
        //         if (staticField.StaticValue is BaseManyToOneIndex staticTree)
        //         {
        //             var realRowManager = (RowManager<Key32>)(object)RowManager;
        //             var classKey = (Key32)(object)vltClass.Key;
        //             
        //             BaseManyToOneIndex.TreeNodeType? nodeType = null;
        //             for (var i = 0; i < staticTree.Keys.Count; i++)
        //             {
        //                 var key = staticTree.Keys[i];
        //                 var indexTableEntry = staticTree.Indices[i];
        //                 var values = staticTree.Values.GetRange(indexTableEntry.Index, indexTableEntry.Count);
        //
        //                 var keyToName = HashManager.ResolveVlt(key);
        //
        //                 if (key != 0)
        //                 {
        //                     var collection = realRowManager.FindCollection(classKey, new Key32(key));
        //
        //                     if (collection == null)
        //                     {
        //                         throw new InvalidDataException(
        //                             $"static index references nonexistent collection: {keyToName}");
        //                     }
        //
        //                     if (values.Count == 1)
        //                     {
        //                         var linkedKey = values[0];
        //                         var linkedCollection = realRowManager.FindCollection(classKey, new Key32(linkedKey));
        //
        //                         if (ReferenceEquals(collection.Parent, linkedCollection))
        //                         {
        //                             if (nodeType == null)
        //                             {
        //                                 nodeType = BaseManyToOneIndex.TreeNodeType.ParentKey;
        //                             }
        //                             else if (nodeType != BaseManyToOneIndex.TreeNodeType.ParentKey)
        //                             {
        //                                 throw new Exception("strange mixture of nodes in static index");
        //                             }
        //                         }
        //                         else
        //                         {
        //                             nodeType = BaseManyToOneIndex.TreeNodeType.ChildKeys;
        //                         }
        //                     }
        //                     else if (nodeType == BaseManyToOneIndex.TreeNodeType.ParentKey)
        //                     {
        //                         throw new Exception("each node in a ParentKey index must have exactly 1 value");
        //                     }
        //                     else
        //                     {
        //                         nodeType = BaseManyToOneIndex.TreeNodeType.ChildKeys;
        //                     }
        //                 }
        //             }
        //
        //             staticTree.NodeType = nodeType ?? BaseManyToOneIndex.TreeNodeType.ChildKeys;
        //         }
        //     }
        // }
    }

    #region Internal Data Reading

    private void ReadExports(VaultReadContext<TKey> context, BinaryReader vltStreamReader, BinaryReader binStreamReader)
    {
        foreach (BaseExport<TKey> vaultExport in context.Exports)
        {
            vltStreamReader.BaseStream.Position = vaultExport.Offset;
            vaultExport.Read(context, vltStreamReader);
#if DEBUG
            if ((vltStreamReader.BaseStream.Position - vaultExport.Offset) != vaultExport.Size)
                throw new Exception();
#endif

            if (vaultExport is IPointerObject<TKey> pointerObject)
            {
                pointerObject.ReadPointerData(context, binStreamReader);
            }

            if (vaultExport is BaseCollectionLoad<TKey> bcl)
            {
                if (bcl.ParentKey != TKey.Zero)
                {
                    _parentKeyDictionary[bcl.Collection] = bcl.ParentKey;
                }
            }
        }

        context.Vault.IsPrimaryVault = context.Exports.OfType<BaseClassLoad<TKey>>().Any();
    }

    private void fixPointers(VaultReadContext<TKey> context, VltPointerType pointerType, Stream stream)
    {
        IEnumerable<VltPointer> pointers =
            from pointer in context.Pointers where pointer.Type == pointerType select pointer;

        ByteOrder byteOrder = context.ByteOrder;
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

    private void processBinChunks(VaultReadContext<TKey> context, ChunkReader<TKey> chunkReader)
    {
        chunkReader.NextChunk().Read(context, chunkReader.Reader);
    }

    private void processVltChunks(VaultReadContext<TKey> context, ChunkReader<TKey> chunkReader)
    {
        while (chunkReader.Reader.BaseStream.Position < chunkReader.Reader.BaseStream.Length)
        {
            var chunk = chunkReader.NextChunk();
            chunk.Read(context, chunkReader.Reader);
            chunk.GoToEnd(chunkReader.Reader.BaseStream);
        }
    }

    #endregion
}