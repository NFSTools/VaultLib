// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:46 AM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;

namespace VaultLib.Core;

/// <summary>
///     Provides utilities for the saving process
/// </summary>
public class VaultWriteContext<TKey> where TKey : struct, IKey<TKey>
{
    public VaultWriteOptions Options { get; }

    public Database<TKey> Database { get; }

    public Vault<TKey> Vault { get; }

    /// <summary>
    /// A set containing every string value in the vault's data.
    /// </summary>
    public HashSet<string> Strings { get; set; }

    /// <summary>
    /// A list of collections in the vault.
    /// </summary>
    public IList<VltCollection<TKey>> Collections { get; set; }

    /// <summary>
    /// A set of pointers for vault data.
    /// </summary>
    public HashSet<VltPointer> Pointers { get; set; }

    /// <summary>
    /// A mapping of string values to data offsets, for pointer generation.
    /// </summary>
    public Dictionary<string, long> StringOffsets { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="VaultWriteContext{TKey}"/> class.
    /// </summary>
    /// <param name="vault"></param>
    /// <param name="options">The options to use in the saving process.</param>
    public VaultWriteContext(Vault<TKey> vault, VaultWriteOptions options)
    {
        Database = vault.Database;
        Vault = vault;
        Options = options;
        Collections = vault.Database.RowManager.GetCollectionsInVault(vault).ToList();
        Pointers = new HashSet<VltPointer>(VltPointer.FixUpOffsetDestinationTypeComparer);
        Strings = new HashSet<string>();
        StringOffsets = new Dictionary<string, long>();
    }

    /// <summary>
    /// Adds a pointer from the given source offset to the given destination offset.
    /// </summary>
    /// <param name="src">The pointer source offset.</param>
    /// <param name="dst">The pointer destination offset.</param>
    /// <param name="isVlt">Whether the pointer is a VLT pointer.</param>
    /// <exception cref="Exception">if a duplicate pointer is added</exception>
    public void AddPointer(long src, long dst, bool isVlt)
    {
        Debug.Assert(src != 0);

        var pointer = new VltPointer
        {
            Type = isVlt ? VltPointerType.Vlt : VltPointerType.Bin,
            FixUpOffset = (uint)src,
            Destination = (uint)dst
        };

        if (!Pointers.Add(pointer)) throw new Exception("Duplicate pointer added?");
    }

    /// <summary>
    /// Computes the appropriate string hash value for the given input text.
    /// </summary>
    /// <param name="text">The text to be hashed.</param>
    /// <returns>The string hash value.</returns>
    /// <remarks>As of VaultLib 3.0, strings beginning with "0x" will NOT be interpreted as hexadecimal numbers.</remarks>
    public TKey StringToKey(string text)
    {
        return TKey.FromString(text);
    }

    public void WriteString(string str, FieldReadWriteContext<TKey> fieldContext, BinaryWriter bw)
    {
        if (!StringOffsets.TryGetValue(str, out var strPtr))
            throw new KeyNotFoundException($"String offset table does not have an entry for: {str}");

        var ptrPos = bw.BaseStream.Position;

        bw.Write(0u);

        AddPointer(ptrPos, strPtr, fieldContext.IsInVlt);
    }
}