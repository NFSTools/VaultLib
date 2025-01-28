// This file is part of VaultLib by heyitsleo.
// 
// Created: 09/24/2019 @ 5:07 PM.

using System.IO;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Exports;

/// <summary>
///     An export is an object that describes an entity in the VLT database.
///     For example, a class is described by a "ClassLoadData" export.
/// </summary>
public abstract class BaseExport<TKey> : IVaultFileAccess<TKey> where TKey : struct, IKey<TKey>
{
    /// <summary>
    ///     The offset of the export data in the VLT stream.
    /// </summary>
    public uint Offset { get; set; }
    /// <summary>
    ///     The length of the export data in the VLT stream.
    /// </summary>
    public uint Size { get; set; }

    public abstract void Read(VaultReadContext<TKey> context, BinaryReader br);
    public abstract void Write(VaultWriteContext<TKey> context, BinaryWriter bw);

    /// <summary>
    ///     Perform any necessary preparation work before data is read.
    /// </summary>
    public virtual void PrepareRead(Vault<TKey> vault)
    {

    }

    /// <summary>
    ///     Perform any necessary preparation work before data is written.
    /// </summary>
    public virtual void Prepare(Vault<TKey> vault)
    {
    }

    /// <summary>
    ///     Retrieve a unique key for the export.
    /// </summary>
    /// <returns>The export's unique key.</returns>
    public abstract TKey GetExportId();

    /// <summary>
    ///     Retrieve the type ID for the export.
    /// </summary>
    /// <returns>The export's type ID.'</returns>
    public abstract string GetTypeId();
}