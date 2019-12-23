// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 3:57 PM.

using CoreLibraries.IO;
using System;
using System.IO;

namespace VaultLib.Core
{
    /// <summary>
    ///     Holds instances of <see cref="BinaryReader" /> for vault loading
    /// </summary>
    public class VaultLoadingWrapper : IDisposable
    {
        /// <summary>
        ///     Initializes the loading wrapper with the given vault.
        /// </summary>
        /// <param name="vault">The vault to create readers for.</param>
        /// <param name="byteOrder">The byte order of the data streams.</param>
        public VaultLoadingWrapper(Vault vault, ByteOrder byteOrder = ByteOrder.Little)
        {
            Vault = vault;
            ByteOrder = byteOrder;
            BinReader = byteOrder == ByteOrder.Little
                ? new BinaryReader(vault.BinStream)
                : new BigEndianBinaryReader(vault.BinStream);
            VltReader = byteOrder == ByteOrder.Little
                ? new BinaryReader(vault.VltStream)
                : new BigEndianBinaryReader(vault.VltStream);

            Vault.ByteOrder = byteOrder;
        }

        public Vault Vault { get; }

        public BinaryReader BinReader { get; }
        public BinaryReader VltReader { get; }

        public ByteOrder ByteOrder { get; }

        public void Dispose()
        {
            BinReader?.Dispose();
            VltReader?.Dispose();
        }
    }
}