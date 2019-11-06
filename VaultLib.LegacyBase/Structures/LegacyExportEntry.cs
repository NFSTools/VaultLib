using System.IO;
using VaultLib.Core;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.LegacyBase.Structures
{
    public class LegacyExportEntry : IExportEntry
    {
        public void Read(Vault vault, BinaryReader br)
        {
            ID = br.ReadUInt32();
            Type = br.ReadUInt32();
            if (br.ReadUInt32() != 0)
                throw new InvalidDataException();
            Size = br.ReadUInt32();
            Offset = br.ReadUInt32();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write((uint) ID);
            bw.Write((uint) Type);
            bw.Write(0);
            bw.Write(Size);
            bw.Write(Offset);
        }

        public ulong ID { get; set; }
        public ulong Type { get; set; }
        public uint Size { get; set; }
        public uint Offset { get; set; }
    }
}