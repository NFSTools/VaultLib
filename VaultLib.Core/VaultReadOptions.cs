using CoreLibraries.IO;

namespace VaultLib.Core;

/// <summary>
/// Options for handling various "quirks" of different versions of the VLT format
/// </summary>
public class VaultReadQuirks
{
    //
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