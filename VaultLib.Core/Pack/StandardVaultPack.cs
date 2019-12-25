// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:57 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.IO;
using VaultLib.Core.DB;
using VaultLib.Core.Pack.Structures;
using VaultLib.Core.Writer;

namespace VaultLib.Core.Pack
{
    /// <summary>
    ///     Implements the classic "VPAK" vault pack.
    /// </summary>
    public class StandardVaultPack : IVaultPack
    {
        public IList<Vault> Load(BinaryReader br, Database database, PackLoadingOptions loadingOptions)
        {
            ByteOrder byteOrder = loadingOptions?.ByteOrder ?? ByteOrder.Little;

            // check for VPAK header
            AttribVaultPackImage vaultPackImage = new AttribVaultPackImage();
            vaultPackImage.Read(null, br);

            br.BaseStream.Position = vaultPackImage.Header.StringBlockOffset;

            List<Vault> vaults = new List<Vault>();

            foreach (AttribVaultPackEntry attribVaultPackEntry in vaultPackImage.Entries)
            {
                br.BaseStream.Position = vaultPackImage.Header.StringBlockOffset + attribVaultPackEntry.VaultNameOffset;

                Vault vault = new Vault(NullTerminatedString.Read(br));

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

                vault.BinStream = new MemoryStream(binData);
                vault.VltStream = new MemoryStream(vltData);

                using (VaultLoadingWrapper loadingWrapper = new VaultLoadingWrapper(vault, byteOrder))
                {
                    database.LoadVault(vault, loadingWrapper);
                }

                vaults.Add(vault);
            }

            return vaults;
        }

        public void Save(BinaryWriter bw, IList<Vault> vaults, PackSavingOptions savingOptions = null)
        {
            Dictionary<string, VaultStreamInfo> streamDictionary = new Dictionary<string, VaultStreamInfo>();

            foreach (var vault in vaults)
            {
                VaultWriter vaultWriter = new VaultWriter(vault, new VaultSaveOptions());
                streamDictionary[vault.Name] = vaultWriter.BuildVault();
            }

            // empty header for now
            bw.Write(new byte[16]);

            Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
            int nameOffset = 0;

            foreach (var databaseVault in vaults)
            {
                nameOffsets[databaseVault.Name] = nameOffset;
                nameOffset += databaseVault.Name.Length + 1;
            }

            bw.Write(nameOffset);

            bw.Write(new byte[20 * vaults.Count]);

            bw.AlignWriter(0x40);

            var nameTablePos = bw.BaseStream.Position;

            foreach (var databaseVault in vaults)
            {
                NullTerminatedString.Write(bw, databaseVault.Name);
            }

            bw.AlignWriter(0x80);

            List<long> binOffsets = new List<long>();
            List<long> vltOffsets = new List<long>();

            foreach (var vault in vaults)
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
                NumEntries = (uint)vaults.Count,
                StringBlockOffset = (uint)nameTablePos,
                StringBlockSize = (uint)nameOffset
            };

            vpi.Header = header;
            vpi.Entries = new List<AttribVaultPackEntry>();

            for (int i = 0; i < vaults.Count; i++)
            {
                Vault vault = vaults[i];
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

            vpi.Write(null, bw);
        }
    }
}
