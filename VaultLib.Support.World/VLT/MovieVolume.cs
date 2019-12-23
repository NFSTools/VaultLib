using CoreLibraries.IO;
using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(MovieVolume))]
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

        public MovieVolume(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public MovieVolume(VltClass @class, VltClassField field) : base(@class, field)
        {
        }
    }
}