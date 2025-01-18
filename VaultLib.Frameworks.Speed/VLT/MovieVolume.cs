using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(MovieVolume))]
    public class MovieVolume : VltBaseType
    {
        public uint Hash { get; set; }
        public byte Volume { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Hash = br.ReadUInt32();
            Volume = br.ReadByte();
            br.AlignReader(4);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Hash);
            bw.Write(Volume);
            bw.AlignWriter(4);
        }
    }
}