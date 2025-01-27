using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.LegacyBase.Structures;

public class ExportEntry32 : IExportEntry<uint>
{
    public void Read(VaultReadContext<uint> context, BinaryReader br)
    {
        Id = br.ReadUInt32();
        Type = br.ReadUInt32();
        if (br.ReadUInt32() != 0)
            throw new InvalidDataException();
        Size = br.ReadUInt32();
        Offset = br.ReadUInt32();
    }

    public void Write(VaultWriteContext<uint> context, BinaryWriter bw)
    {
        bw.Write(Id);
        bw.Write(Type);
        bw.Write(0);
        bw.Write(Size);
        bw.Write(Offset);
    }

    public uint Id { get; set; }
    public uint Type { get; set; }
    public uint Size { get; set; }
    public uint Offset { get; set; }
}