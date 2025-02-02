// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:57 PM.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.DB;
using VaultLib.Core.Pack.Structures;
using VaultLib.Core.Writer;

namespace VaultLib.Core.Pack;

/// <summary>
///     Implements the classic "VPAK" vault pack.
/// </summary>
public class StandardVaultPack : IVaultPack
{
    public IList<Vault<TKey>> Load<TKey>(BinaryReader br, Database<TKey> database, PackLoadingOptions? loadingOptions)
        where TKey : struct, IKey<TKey>
    {
        ByteOrder byteOrder = loadingOptions?.ByteOrder ?? ByteOrder.Little;

        // check for VPAK header
        AttribVaultPackImage vaultPackImage = new AttribVaultPackImage();
        vaultPackImage.Read(br);

        br.BaseStream.Position = vaultPackImage.Header.StringBlockOffset;

        List<Vault<TKey>> vaults = new();

        foreach (AttribVaultPackEntry attribVaultPackEntry in vaultPackImage.Entries)
        {
            br.BaseStream.Position = vaultPackImage.Header.StringBlockOffset + attribVaultPackEntry.VaultNameOffset;

            var vaultName = NullTerminatedString.Read(br);
            br.BaseStream.Seek(attribVaultPackEntry.BinOffset, SeekOrigin.Begin);

            byte[] binData = new byte[attribVaultPackEntry.BinSize];

            if (br.Read(binData, 0, binData.Length) != binData.Length)
            {
                throw new Exception($"Failed to read {binData.Length} bytes of BIN data");
            }

            br.BaseStream.Seek(attribVaultPackEntry.VltOffset, SeekOrigin.Begin);

            byte[] vltData = new byte[attribVaultPackEntry.VltSize];

            if (br.Read(vltData, 0, vltData.Length) != vltData.Length)
            {
                throw new Exception($"Failed to read {vltData.Length} bytes of VLT data");
            }

            var binStream = new MemoryStream(binData);
            var vltStream = new MemoryStream(vltData);

            using var readWrapper = new VaultReadWrapper(vaultName, binStream, vltStream, byteOrder);
            var vault = database.LoadVault(readWrapper);
            vaults.Add(vault);
        }

        return vaults;
    }

    public void Save<TKey>(BinaryWriter bw, IList<Vault<TKey>> vaults, PackSavingOptions? savingOptions = null)
        where TKey : struct, IKey<TKey>
    {
        var filteredAndSortedVaults =
            vaults.Where(v => v.Database.RowManager.GetCollectionsInVault(v).Any()).ToList();
        filteredAndSortedVaults.Sort((x, y) => string.CompareOrdinal(x.Name, y.Name));
        Dictionary<string, VaultStreamInfo> streamDictionary = new Dictionary<string, VaultStreamInfo>();

        var vaultWriteOptions = savingOptions?.VaultWriteOptions ?? new VaultWriteOptions();
        foreach (var vault in filteredAndSortedVaults)
        {
            VaultWriter<TKey> vaultWriter =
                new VaultWriter<TKey>(vault, vaultWriteOptions);
            streamDictionary[vault.Name] = vaultWriter.BuildVault();
        }

        // empty header for now
        bw.Write(new byte[16]);

        Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
        int nameOffset = 0;

        foreach (var databaseVault in filteredAndSortedVaults)
        {
            nameOffsets[databaseVault.Name] = nameOffset;
            nameOffset += databaseVault.Name.Length + 1;
        }

        bw.Write(nameOffset);

        bw.Write(new byte[20 * filteredAndSortedVaults.Count]);

        bw.AlignWriter(0x40);

        var nameTablePos = bw.BaseStream.Position;

        foreach (var databaseVault in filteredAndSortedVaults)
        {
            NullTerminatedString.Write(bw, databaseVault.Name);
        }

        bw.AlignWriter(0x80);

        List<long> binOffsets = new List<long>();
        List<long> vltOffsets = new List<long>();

        foreach (var vault in filteredAndSortedVaults)
        {
            bw.AlignWriter(0x80);
            var streamInfo = streamDictionary[vault.Name];

            binOffsets.Add(bw.BaseStream.Position);
            streamInfo.BinStream.CopyTo(bw.BaseStream);

            bw.AlignWriter(0x80);

            vltOffsets.Add(bw.BaseStream.Position);
            streamInfo.VltStream.CopyTo(bw.BaseStream);
        }

        bw.BaseStream.Position = 0;

        // write header
        AttribVaultPackImage vpi = new AttribVaultPackImage();
        AttribVaultPackHeader header = new AttribVaultPackHeader
        {
            NumEntries = (uint)filteredAndSortedVaults.Count,
            StringBlockOffset = (uint)nameTablePos,
            StringBlockSize = (uint)nameOffset
        };

        vpi.Header = header;
        vpi.Entries = new List<AttribVaultPackEntry>();

        for (var i = 0; i < filteredAndSortedVaults.Count; i++)
        {
            var vault = filteredAndSortedVaults[i];
            var streamInfo = streamDictionary[vault.Name];
            AttribVaultPackEntry entry = new AttribVaultPackEntry
            {
                BinOffset = (uint)binOffsets[i],
                VltOffset = (uint)vltOffsets[i],
                BinSize = (uint)streamInfo.BinStream.Length,
                VltSize = (uint)streamInfo.VltStream.Length,
                VaultNameOffset = (uint)nameOffsets[vault.Name]
            };

            vpi.Entries.Add(entry);
        }

        vpi.Write(bw);
    }
}