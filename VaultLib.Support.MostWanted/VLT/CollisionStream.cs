using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.MostWanted.VLT
{
    [VLTTypeInfo(nameof(CollisionStream))]
    public class CollisionStream : VLTBaseType
    {
        public CollisionStream(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public CollisionStream(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RefSpec StreamMoment { get; set; }
        public byte Threshold { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            StreamMoment = new RefSpec(Class, Field, Collection);
            StreamMoment.Read(vault, br);
            Threshold = br.ReadByte();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            StreamMoment.Write(vault, bw);
            bw.Write(Threshold);
            bw.AlignWriter(4);
        }
    }
}