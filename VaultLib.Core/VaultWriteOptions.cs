using VaultLib.Core.Chunks;

namespace VaultLib.Core;

/// <summary>
/// An enumeration of the available stringhash implementations.
/// </summary>
public enum VaultHashMode
{
    Hash32,
    Hash64
}

/// <summary>
/// Options for handling various "quirks" of different versions of the VLT format
/// </summary>
public class VaultWriteQuirks
{
    /// <summary>
    /// When set to true, the <see cref="VltStartChunk{TKey}"/> will be written before the <see cref="VltDependencyChunk{TKey}"/>.
    /// </summary>
    /// <remarks>
    /// This is mainly useful for ensuring compatibility with NFS-VltEd. It "should" be set for Undercover and World
    /// vaults, but the chunk order doesn't seem to matter to any of the games.
    /// </remarks>
    public bool StartChunkBeforeDepChunk { get; set; }

    /// <summary>
    /// When set to true, the <see cref="EndChunk{TKey}"/> will be written to the BIN stream.
    /// </summary>
    public bool EnableBinEndChunk { get; set; }
}

/// <summary>
/// Options for the vault saving process
/// </summary>
public class VaultWriteOptions
{
    /// <summary>
    /// The string hash implementation to use.
    /// </summary>
    public VaultHashMode HashMode { get; init; } = VaultHashMode.Hash32;

    /// <summary>
    /// The quirks to apply.
    /// </summary>
    public VaultWriteQuirks Quirks { get; init; } = new();
}