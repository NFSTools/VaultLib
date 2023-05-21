using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(UpgradeSpecs))]
    public class UpgradeSpecs : VLTBaseType
    {
        public RefSpec ReferencedRow { get; set; }

        public uint UpgradeLevel { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            ReferencedRow.Read(vault, br);
            UpgradeLevel = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ReferencedRow.Write(vault, bw);
            bw.Write(UpgradeLevel);
        }

        public UpgradeSpecs(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            ReferencedRow = new RefSpec(Class, Field, Collection);
        }
    }
}