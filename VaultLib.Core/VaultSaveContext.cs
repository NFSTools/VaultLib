// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:46 AM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using VaultLib.Core.Data;
using VLT32Hasher = VaultLib.Core.Utils.VLT32Hasher;
using VLT64Hasher = VaultLib.Core.Utils.VLT64Hasher;

namespace VaultLib.Core
{
    /// <summary>
    ///     Provides utilities for the saving process
    /// </summary>
    public class VaultSaveContext
    {
        private VaultSaveOptions Options { get; }

        /// <summary>
        /// A set containing every string value in the vault's data.
        /// </summary>
        public HashSet<string> Strings { get; set; }

        /// <summary>
        /// A list of <see cref="VltCollection"/> instances in the vault.
        /// </summary>
        public IList<VltCollection> Collections { get; set; }

        /// <summary>
        /// A set of <see cref="VltPointer"/> instances for vault data.
        /// </summary>
        public HashSet<VltPointer> Pointers { get; set; }

        /// <summary>
        /// A mapping of string values to data offsets, for pointer generation.
        /// </summary>
        public Dictionary<string, long> StringOffsets { get; set; }

        public VaultHashMode HashMode => Options.HashMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="VaultSaveContext"/> class.
        /// </summary>
        /// <param name="options">The options to use in the saving process.</param>
        public VaultSaveContext(VaultSaveOptions options)
        {
            Options = options;
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
        /// <remarks>Strings beginning with "0x" will be converted to numeric values.</remarks>
        public ulong StringHash(string text)
        {
            if (text.StartsWith("0x") && ulong.TryParse(text.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out ulong l))
            {
                return l;
            }

            switch (Options.HashMode)
            {
                case VaultHashMode.Hash32:
                    return VLT32Hasher.Hash(text);
                case VaultHashMode.Hash64:
                    return VLT64Hasher.Hash(text);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}