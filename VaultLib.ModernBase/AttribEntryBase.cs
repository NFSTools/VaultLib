using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Utils;
using VaultLib.ModernBase.Exports;

namespace VaultLib.ModernBase;

public abstract class AttribEntryBase : IVaultFileAccess, IPointerObject
{
    public ulong Key { get; set; }
    public ushort TypeIndex { get; set; }
    public NodeFlagsEnum NodeFlags { get; set; }
    public byte EntryFlags { get; set; }
    public long InlineDataPointer { get; set; }
    public object InlineData { get; set; }
    public VltCollection Collection { get; }

    protected AttribEntryBase(VltCollection collection)
    {
        Collection = collection;
    }

    public abstract void AddPointers(VaultWriteContext context);
    public abstract void Read(VaultReadContext context, BinaryReader br);
    public abstract void ReadPointerData(VaultReadContext context, BinaryReader br);
    public abstract void Write(VaultWriteContext context, BinaryWriter bw);
    public abstract void WritePointerData(VaultWriteContext context, BinaryWriter bw);
}