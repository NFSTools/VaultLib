using CoreLibraries.IO;
using VaultLib.Core.Chunks;

namespace VaultLib.Core;

/// <summary>
/// Options for handling various "quirks" of different versions of the VLT format
/// </summary>
public class VaultReadQuirks
{
    /// <summary>
    /// When set to true, the <see cref="VltStartChunk"/> will be written before the <see cref="VltDependencyChunk"/>.
    /// </summary>
    /// <remarks>
    /// This is mainly useful for ensuring compatibility with NFS-VltEd. It "should" be set for Undercover and World
    /// vaults, but the chunk order doesn't seem to matter to any of the games.
    /// </remarks>
    public bool StartChunkBeforeDepChunk { get; set; }
    
    /// <summary>
    /// When set to true, the <see cref="EndChunk"/> will be written to the BIN stream.
    /// </summary>
    public bool EnableBinEndChunk { get; set; }
}

/// <summary>
/// Options for the vault reading process
/// </summary>
public class VaultReadOptions
{
    /// <summary>
    /// The byte order to use when reading the vault.
    /// </summary>
    public ByteOrder ByteOrder { get; set; }

    /// <summary>
    /// The quirks to apply.
    /// </summary>
    public VaultReadQuirks Quirks { get; init; } = new();
}