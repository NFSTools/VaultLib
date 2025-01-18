using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(CPartsPackageEntry))]
    public class CPartsPackageEntry : VltBaseType
    {
        public RefSpec Part { get; set; } = new();
        public byte KitNum { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Part.Read(context, fieldContext, br);
            KitNum = br.ReadByte();
            br.AlignReader(4);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Part.Write(context, fieldContext, bw);
            bw.Write(KitNum);
            bw.AlignWriter(4);
        }
    }
}