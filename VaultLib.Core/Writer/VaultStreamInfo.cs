using System.IO;

namespace VaultLib.Core.Writer
{
    /// <summary>
    /// Simple wrapper for the streams returned by <see cref="VaultWriter"/>.
    /// </summary>
    public class VaultStreamInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VaultStreamInfo"/> class.
        /// </summary>
        /// <param name="binStream">The BIN data stream.</param>
        /// <param name="vltStream">The VLT data stream.</param>
        public VaultStreamInfo(Stream binStream, Stream vltStream)
        {
            BinStream = binStream;
            VltStream = vltStream;
        }

        /// <summary>
        /// Gets the generated BIN stream.
        /// </summary>
        public Stream BinStream { get; }

        /// <summary>
        /// Gets the generated VLT stream.
        /// </summary>
        public Stream VltStream { get; }
    }
}