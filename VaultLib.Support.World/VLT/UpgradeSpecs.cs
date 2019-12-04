using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.World.VLT
{
    [VLTTypeInfo(nameof(UpgradeSpecs))]
    public class UpgradeSpecs : VLTBaseType
    {
        public RefSpec ReferencedRow { get; set; }

        public uint UpgradeLevel { get; set; }
        
        public override void Read(Vault vault, BinaryReader br)
        {
            ReferencedRow = new RefSpec(Class, Field, Collection);
            ReferencedRow.Read(vault, br);
            UpgradeLevel = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            ReferencedRow.Write(vault, bw);
            bw.Write(UpgradeLevel);
        }

        public UpgradeSpecs(VLTClass @class, VLTClassField field, VLTCollection collection) : base(@class, field, collection)
        {
        }

        public UpgradeSpecs(VLTClass @class, VLTClassField field) : base(@class, field)
        {
        }
    }
}