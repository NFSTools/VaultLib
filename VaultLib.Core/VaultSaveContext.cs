// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/30/2019 @ 9:46 AM.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using VaultLib.Core.Data;

namespace VaultLib.Core
{
    /// <summary>
    ///     Data stored and used during the save process
    /// </summary>
    public class VaultSaveContext
    {
        public HashSet<string> Strings { get; set; }

        public HashSet<VltPointer> Pointers { get; set; }

        public List<VltCollection> Collections { get; set; }

        public Dictionary<string, long> StringOffsets { get; set; }

        public void AddPointer(long src, long dst, bool isVlt)
        {
            Debug.Assert(src != 0);

            var pointer = new VltPointer
            {
                Type = isVlt ? VltPointerType.Vlt : VltPointerType.Bin,
                FixUpOffset = (uint) src,
                Destination = (uint) dst
            };

            if (!Pointers.Add(pointer)) throw new Exception("Duplicate pointer added?");
        }
    }
}