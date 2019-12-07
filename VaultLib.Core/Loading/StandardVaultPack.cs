// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:57 PM.

using CoreLibraries.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VaultLib.Core.DB;
using VaultLib.Core.Loading.Structures;

namespace VaultLib.Core.Loading
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

        public void Save(BinaryWriter bw, IList<Vault> vaults)
        {
            AttribVaultPackImage vaultPackImage = new AttribVaultPackImage();
            Dictionary<string, (MemoryStream bin, MemoryStream vlt)> streamDictionary = new Dictionary<string, (MemoryStream bin, MemoryStream vlt)>();

            foreach (var vault in vaults)
            {
                VaultWriter vaultWriter = new VaultWriter(vault);
                streamDictionary[vault.Name] = vaultWriter.Save();
            }

            /*                bw bw = new bw(stream);

                bw.Write(0x4B415056); // VPAK
                bw.Write(file.Vaults.Count);

                var nameTablePtrOffset = bw.BaseStream.Position;
                bw.Write(0);

                //int nameTableLength = database.Vaults.Aggregate(0, (i, v) => i + v.Name.Length + 1);
                Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
                int nameOffset = 0;

                foreach (var databaseVault in file.Vaults)
                {
                    nameOffsets[databaseVault.Name] = nameOffset;
                    nameOffset += databaseVault.Name.Length + 1;
                }

                bw.Write(nameOffset);

                var entryTablePos = bw.BaseStream.Position;

                foreach (var databaseVault in file.Vaults)
                {
                    bw.Write(nameOffsets[databaseVault.Name]);
                    bw.Write((int)databaseVault.BinStream.Length);
                    bw.Write((int)databaseVault.VltStream.Length);
                    bw.Write(0); // bin offset
                    bw.Write(0); // vlt offset
                }

                bw.AlignWriter(0x40);

                var nameTablePos = bw.BaseStream.Position;

                foreach (var databaseVault in file.Vaults)
                {
                    NullTerminatedString.Write(bw, databaseVault.Name);
                }

                bw.AlignWriter(0x80);

                List<long> binOffsets = new List<long>();
                List<long> vltOffsets = new List<long>();

                foreach (var databaseVault in file.Vaults)
                {
                    bw.AlignWriter(0x80);

                    binOffsets.Add(bw.BaseStream.Position);
                    databaseVault.BinStream.CopyTo(bw.BaseStream);

                    bw.AlignWriter(0x80);

                    vltOffsets.Add(bw.BaseStream.Position);
                    databaseVault.VltStream.CopyTo(bw.BaseStream);
                }

                bw.BaseStream.Position = entryTablePos;

                for (var index = 0; index < file.Vaults.Count; index++)
                {
                    var databaseVault = file.Vaults[index];
                    bw.Write(nameOffsets[databaseVault.Name]);
                    bw.Write((int)databaseVault.BinStream.Length);
                    bw.Write((int)databaseVault.VltStream.Length);
                    bw.Write((int)binOffsets[index]); // bin offset
                    bw.Write((int)vltOffsets[index]); // vlt offset
                }

                bw.BaseStream.Position = nameTablePtrOffset;
                bw.Write((int)nameTablePos);
                */

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
                (MemoryStream bin, MemoryStream vlt) streamTuple = streamDictionary[vault.Name];

                binOffsets.Add(bw.BaseStream.Position);
                streamTuple.bin.CopyTo(bw.BaseStream);

                bw.AlignWriter(0x80);

                vltOffsets.Add(bw.BaseStream.Position);
                streamTuple.vlt.CopyTo(bw.BaseStream);
            }

            bw.BaseStream.Position = 0;

            // write header
            AttribVaultPackImage vpi = new AttribVaultPackImage();
            AttribVaultPackHeader header = new AttribVaultPackHeader();
            header.NumEntries = (uint) vaults.Count;
            header.StringBlockOffset = (uint) nameTablePos;
            header.StringBlockSize = (uint) nameOffset;

            vpi.Header = header;
            vpi.Entries = new List<AttribVaultPackEntry>();

            for (int i = 0; i < vaults.Count; i++)
            {
                Vault vault = vaults[i];
                (MemoryStream bin, MemoryStream vlt) streamTuple = streamDictionary[vault.Name];
                AttribVaultPackEntry entry = new AttribVaultPackEntry();
                entry.BinOffset = (uint) binOffsets[i];
                entry.VltOffset = (uint) vltOffsets[i];
                entry.BinSize = (uint) streamTuple.bin.Length;
                entry.VltSize = (uint) streamTuple.vlt.Length;
                entry.VaultNameOffset = (uint) nameOffsets[vault.Name];

                vpi.Entries.Add(entry);
            }

            vpi.Write(null, bw);
        }
    }
}
 