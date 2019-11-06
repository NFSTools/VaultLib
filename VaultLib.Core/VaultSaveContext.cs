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
    /// Data stored and used during the save process
    /// </summary>
    public class VaultSaveContext
    {
        public HashSet<string> Strings { get; set; }

        public HashSet<VLTPointer> Pointers { get; set; }

        public List<VLTCollection> Collections { get; set; }

        public Dictionary<string, long> StringOffsets { get; set; }

        public void AddPointer(long src, long dst, bool isVLT)
        {
            Debug.Assert(src != 0 );

            VLTPointer pointer = new VLTPointer
            {
                Type = isVLT ? VLTPointerType.Vlt : VLTPointerType.Bin,
                FixUpOffset = (uint)src,
                Destination = (uint)dst
            };

            if (!Pointers.Add(pointer))
            {
                throw new Exception("Duplicate pointer added?");
            }
        }
    }
}