using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(CollisionStream))]
    public class CollisionStream : VLTBaseType
    {
        public CollisionStream(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            StreamMoment = new RefSpec(Class, Field, Collection);
        }

        public RefSpec StreamMoment { get; set; }
        public byte Threshold { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
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