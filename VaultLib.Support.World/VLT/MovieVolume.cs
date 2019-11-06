using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    public class MovieVolume : VLTBaseType
    {
        public uint Hash { get; set; }
        public byte Volume { get; set; }
        
        public override void Read(Vault vault, BinaryReader br)
        {
            Hash = br.ReadUInt32();
            Volume = br.ReadByte();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(Hash);
            bw.Write(Volume);
            bw.AlignWriter(4);
        }
    }
}