using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Chunks
{
    public abstract class ChunkBase : IVaultFileAccess
    {
        public abstract uint Id { get; }
        public abstract uint Size { get; set; }
        public abstract long Offset { get; set; }
        public long EndOffset => Offset + Size;

        public abstract void Read(Vault vault, BinaryReader br);
        public abstract void Write(VaultSaveContext context, BinaryWriter bw);

        /// <summary>
        ///     Proxy method: Jump to the end of the chunk
        /// </summary>
        /// <param name="stream"></param>
        public void GoToEnd(Stream stream)
        {
            stream.Position = Offset + Size;
        }
    }
}