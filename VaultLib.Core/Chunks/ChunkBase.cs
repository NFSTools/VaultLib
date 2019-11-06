using System.IO;
using VaultLib.Core.Utils;

namespace VaultLib.Core.Chunks
{
    public abstract class ChunkBase : IFileAccess
    {
        public abstract uint ID { get; }
        public abstract uint Size { get; set; }
        public abstract long Offset { get; set; }
        public long EndOffset => Offset + Size;

        /// <summary>
        /// Proxy method: Jump to the end of the chunk
        /// </summary>
        /// <param name="stream"></param>
        public void GoToEnd(Stream stream)
        {
            stream.Position = Offset + Size;
        }

        public abstract void Read(Vault vault, BinaryReader br);
        public abstract void Write(Vault vault, BinaryWriter bw);
    }
}
