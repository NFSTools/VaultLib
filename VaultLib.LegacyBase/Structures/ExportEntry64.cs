using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.LegacyBase.Structures
{
    public class ExportEntry64 : IExportEntry
    {
        public void Read(Vault vault, BinaryReader br)
        {
            ID = br.ReadUInt64();
            Type = br.ReadUInt64();
            if (br.ReadUInt32() != 0)
                throw new InvalidDataException();
            Size = br.ReadUInt32();
            Offset = br.ReadUInt32();
            if (br.ReadUInt32() != 0)
                throw new InvalidDataException();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Type);
            bw.Write(0);
            bw.Write(Size);
            bw.Write(Offset);
            bw.Write(0);
        }

        public ulong ID { get; set; }
        public ulong Type { get; set; }
        public uint Size { get; set; }
        public uint Offset { get; set; }
    }
}