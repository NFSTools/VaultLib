// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:46 AM.

using System.Collections.Generic;
using System.IO;
using VaultLib.Core.Data;
using VaultLib.Core.DB;

namespace VaultLib.Core
{
    /// <summary>
    ///     Provides utilities for the saving process
    /// </summary>
    public class VaultReadContext
    {
        public Database Database { get; }

        public Vault Vault { get; }

        public Dictionary<long, string> Strings { get; }

        /// <summary>
        ///     The data pointers.
        /// </summary>
        public List<VltPointer> Pointers { get; }

        /// <summary>
        ///     The BIN data stream, where most of the actual data lies.
        /// </summary>
        public Stream BinStream { get; }

        /// <summary>
        ///     The VLT data stream, where most of the information lies. (Some is in BIN. Why?!)
        /// </summary>
        public Stream VltStream { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultReadContext"/> class.
        /// </summary>
        /// <param name="vault"></param>
        /// <param name="binStream"></param>
        /// <param name="vltStream"></param>
        public VaultReadContext(Vault vault, Stream binStream, Stream vltStream)
        {
            Database = vault.Database;
            Vault = vault;
            BinStream = binStream;
            VltStream = vltStream;
            Strings = new Dictionary<long, string>();
            Pointers = new List<VltPointer>();
        }

        public string ReadString(BinaryReader binaryReader)
        {
            var ptr = binaryReader.ReadUInt32();

            if (!Strings.TryGetValue(ptr, out var value))
            {
                throw new InvalidDataException($"Could not find string at {ptr}");
            }

            return value;
        }
    }
}