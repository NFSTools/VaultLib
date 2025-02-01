using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.LegacyBase.Structures;

public class ExportEntry64 : IExportEntry<Key64>
{
    public void Read(VaultReadContext<Key64> context, BinaryReader br)
    {
        Id = new Key64(br.ReadUInt64());
        Type = new Key64(br.ReadUInt64());
        if (br.ReadUInt32() != 0)
            throw new InvalidDataException();
        Size = br.ReadUInt32();
        Offset = br.ReadUInt32();
        if (br.ReadUInt32() != 0)
            throw new InvalidDataException();
    }

    public void Write(VaultWriteContext<Key64> context, BinaryWriter bw)
    {
        bw.Write(Id.Hash);
        bw.Write(Type.Hash);
        bw.Write(0);
        bw.Write(Size);
        bw.Write(Offset);
        bw.Write(0);
    }

    public Key64 Id { get; set; }
    public Key64 Type { get; set; }
    public uint Size { get; set; }
    public uint Offset { get; set; }
}