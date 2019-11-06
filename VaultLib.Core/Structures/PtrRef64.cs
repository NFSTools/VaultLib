using System.IO;
using CoreLibraries.IO;
using VaultLib.Core.DataInterfaces;

namespace VaultLib.Core.Structures
{
    public class PtrRef64 : IPtrRef
    {
        public void Read(Vault vault, BinaryReader br)
        {
            FixupOffset = br.ReadUInt32();
            PtrType = br.ReadEnum<EPtrRefType>();
            Index = br.ReadUInt16();
            Destination = br.ReadUInt32();
            br.ReadUInt32();
        }

        public void Write(Vault vault, BinaryWriter bw)
        {
            throw new System.NotImplementedException();
        }

        public uint FixupOffset { get; set; }
        public EPtrRefType PtrType { get; set; }
        public ushort Index { get; set; }
        public uint Destination { get; set; }
    }
}