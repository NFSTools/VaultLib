using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(StitchCollisionVol))]
    public class StitchCollisionVol : VltBaseType
    {
        public short Vol1 { get; set; }
        public short Vol2 { get; set; }
        public short Vol3 { get; set; }
        public short Vol4 { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Vol1 = br.ReadInt16();
            Vol2 = br.ReadInt16();
            Vol3 = br.ReadInt16();
            Vol4 = br.ReadInt16();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(Vol1);
            bw.Write(Vol2);
            bw.Write(Vol3);
            bw.Write(Vol4);
        }
    }
}