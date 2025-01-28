using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.DataInterfaces;
using VaultLib.Core.Utils;
using VaultLib.ModernBase.Exports;

namespace VaultLib.ModernBase;

public abstract class AttribEntryBase<TKey> : IVaultFileAccess<TKey>, IPointerObject<TKey> where TKey : struct, IKey<TKey>
{
    public TKey Key { get; set; }
    public ushort TypeIndex { get; set; }
    public NodeFlagsEnum NodeFlags { get; set; }
    public byte EntryFlags { get; set; }
    public long InlineDataPointer { get; set; }
    public object InlineData { get; set; }
    public VltCollection<TKey> Collection { get; }

    protected AttribEntryBase(VltCollection<TKey> collection)
    {
        Collection = collection;
    }

    public abstract void AddPointers(VaultWriteContext<TKey> context);
    public abstract void Read(VaultReadContext<TKey> context, BinaryReader br);
    public abstract void ReadPointerData(VaultReadContext<TKey> context, BinaryReader br);
    public abstract void Write(VaultWriteContext<TKey> context, BinaryWriter bw);
    public abstract void WritePointerData(VaultWriteContext<TKey> context, BinaryWriter bw);
}