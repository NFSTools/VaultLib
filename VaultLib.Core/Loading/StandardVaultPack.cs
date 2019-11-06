// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:57 PM.

using System;
using System.Collections.Generic;
using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DB;
using VaultLib.Core.Loading.Structures;
using VaultLib.Core.Structures;

namespace VaultLib.Core.Loading
{
    /// <summary>
    /// Implements the classic "VPAK" vault pack.
    /// </summary>
    public class StandardVaultPack : IVaultPack
    {
        public void Load(BinaryReader br, Database database, PackLoadingOptions loadingOptions)
        {
            DatabaseLoadedFile loadedFile = new DatabaseLoadedFile();
            ByteOrder byteOrder = loadingOptions?.ByteOrder ?? ByteOrder.Little;

            // check for VPAK header
            AttribVaultPackImage vaultPackImage = new AttribVaultPackImage();
            vaultPackImage.Read(null, br);

            br.BaseStream.Position = vaultPackImage.Header.StringBlockOffset;

            var vaults = new List<Vault>();

            foreach (var attribVaultPackEntry in vaultPackImage.Entries)
            {
                br.BaseStream.Position = vaultPackImage.Header.StringBlockOffset + attribVaultPackEntry.VaultNameOffset;

                var vault = new Vault(NullTerminatedString.Read(br));

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
                    database.LoadVault(loadedFile, vault, loadingWrapper);
                }

                vaults.Add(vault);
            }

            database.LoadFile(loadedFile);
        }
    }
}