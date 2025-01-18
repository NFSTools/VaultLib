namespace VaultLib.Core
{
    /// <summary>
    /// An enumeration of the available stringhash implementations.
    /// </summary>
    public enum VaultHashMode
    {
        Hash32,
        Hash64
    }

    /// <summary>
    /// Options for the vault saving process
    /// </summary>
    public class VaultWriteOptions
    {
        /// <summary>
        /// The string hash implementation to use.
        /// </summary>
        public VaultHashMode HashMode { get; } = VaultHashMode.Hash32;
    }
}
