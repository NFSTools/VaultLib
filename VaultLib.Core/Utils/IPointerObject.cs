// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/28/2019 @ 10:51 AM.

using System.IO;

namespace VaultLib.Core.Utils
{
    public interface IPointerObject
    {
        /// <summary>
        ///     Read data stored through pointers to the BIN stream
        /// </summary>
        /// <param name="vault"></param>
        /// <param name="br"></param>
        void ReadPointerData(Vault vault, BinaryReader br);

        /// <summary>
        ///     Read data stored through pointers to the BIN stream
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bw"></param>
        void WritePointerData(VaultSaveContext context, BinaryWriter bw);

        /// <summary>
        ///     Add pointer information to the vault
        /// </summary>
        /// <param name="context"></param>
        void AddPointers(VaultSaveContext context);
    }
}