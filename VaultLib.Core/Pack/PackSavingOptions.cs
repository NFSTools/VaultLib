// This file is part of VaultLib.Core by heyitsleo.
// 
// Created: 10/31/2019 @ 4:59 PM.

using CoreLibraries.IO;

namespace VaultLib.Core.Pack
{
    /// <summary>
    ///     Provides options for vault pack saving
    /// </summary>
    public class PackSavingOptions
    {
        public PackSavingOptions(ByteOrder byteOrder = ByteOrder.Little)
        {
            ByteOrder = byteOrder;
        }

        public ByteOrder ByteOrder { get; }
    }
}