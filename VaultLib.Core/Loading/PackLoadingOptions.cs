// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:59 PM.

using CoreLibraries.IO;

namespace VaultLib.Core.Loading
{
    /// <summary>
    ///     Provides options for vault pack loading
    /// </summary>
    public class PackLoadingOptions
    {
        public PackLoadingOptions(ByteOrder byteOrder = ByteOrder.Little)
        {
            ByteOrder = byteOrder;
        }

        public ByteOrder ByteOrder { get; }
    }
}