// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/23/2019 @ 9:27 PM.

using System.IO;

namespace VaultLib.Core.Utils
{
    /// <summary>
    ///     Generic interface for reading and writing binary structures.
    /// </summary>
    public interface IFileAccess
    {
        void Read(BinaryReader br);
        void Write(BinaryWriter bw);
    }

    /// <summary>
    ///     Generic interface for reading and writing binary structures, with access to the containing <see cref="Vault"/>.
    /// </summary>
    public interface IVaultFileAccess
    {
        void Read(Vault vault, BinaryReader br);
        void Write(Vault vault, BinaryWriter bw);
    }
}