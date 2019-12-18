// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 5:07 PM.

using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports
{
    /// <summary>
    ///     An export is an object that describes an entity in the VLT database.
    ///     For example, a class is described by a "ClassLoadData" export.
    /// </summary>
    public abstract class BaseExport : IFileAccess
    {
        /// <summary>
        ///     The offset of the export data in the VLT stream.
        /// </summary>
        public uint Offset { get; set; }

        public abstract void Read(Vault vault, BinaryReader br);
        public abstract void Write(Vault vault, BinaryWriter bw);

        /// <summary>
        ///     Perform any necessary preparation work before data is read.
        /// </summary>
        public virtual void PrepareRead(Vault vault)
        {

        }

        /// <summary>
        ///     Perform any necessary preparation work before data is written.
        /// </summary>
        public virtual void Prepare(Vault vault)
        {
        }

        /// <summary>
        ///     Retrieve a unique key for the export.
        /// </summary>
        /// <returns>The export's unique key.</returns>
        public abstract ulong GetExportID();

        /// <summary>
        ///     Retrieve the type ID for the export.
        /// </summary>
        /// <returns>The export's type ID.'</returns>
        public abstract string GetTypeId();
    }
}