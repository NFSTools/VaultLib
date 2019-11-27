// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/14/2019 @ 2:22 PM.

using System.IO;

namespace VaultLib.Core.Utils
{
    public static class BinaryExtensions
    {
        public static uint ReadPointer(this BinaryReader br)
        {
            return br.ReadUInt32();
        }
    }
}