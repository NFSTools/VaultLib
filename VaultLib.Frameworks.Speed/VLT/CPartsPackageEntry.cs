using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(CPartsPackageEntry))]
    public class CPartsPackageEntry : VLTBaseType
    {
        public CPartsPackageEntry(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Part = new RefSpec(Class, Field, Collection);
        }

        public RefSpec Part { get; set; }
        public byte KitNum { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Part.Read(vault, br);
            KitNum = br.ReadByte();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Part.Write(vault, bw);
            bw.Write(KitNum);
            bw.AlignWriter(4);
        }
    }
}