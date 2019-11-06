// This file is part of VaultLib by heyitsleo.
// 
// Created: 10/14/2019 @ 2:22 PM.

using System.IO;

namespace VaultLib.Core.Utils
{
    public static class BinaryExtensions
    {
        public static uint ReadPointer(this BinaryReader br, Vault vault, bool inVLT)
        {
            return br.ReadUInt32();
            //long pos = br.BaseStream.Position;
            //uint result = br.ReadUInt32();

            //if (result != 0)
            //{
            //    VLTPointer pointer = vault.Pointers.First(p =>
            //        p.Type == (inVLT ? VLTPointerType.Vlt : VLTPointerType.Bin) && p.FixUpOffset == pos);

            //    if (pointer.Tracked)
            //        throw new Exception("read the same pointer twice?");

            //    pointer.Tracked = true;
            //}

            //return result;
        }
    }
}