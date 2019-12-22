using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Utils;
using VaultLib.ModernBase.Exports;

namespace VaultLib.ModernBase
{
    public abstract class AttribEntryBase : IFileAccess, IPointerObject
    {
        public ulong Key { get; set; }
        public ushort TypeIndex { get; set; }
        public NodeFlagsEnum NodeFlags { get; set; }
        public byte EntryFlags { get; set; }
        public long InlineDataPointer { get; set; }
        public VLTBaseType InlineData { get; set; }
        public VltCollection Collection { get; }

        protected AttribEntryBase(VltCollection collection)
        {
            Collection = collection;
        }

        public abstract void AddPointers(Vault vault);
        public abstract void Read(Vault vault, BinaryReader br);
        public abstract void ReadPointerData(Vault vault, BinaryReader br);
        public abstract void Write(Vault vault, BinaryWriter bw);
        public abstract void WritePointerData(Vault vault, BinaryWriter bw);
        public abstract bool ReadData(Vault vault, BinaryReader br);
    }
}
