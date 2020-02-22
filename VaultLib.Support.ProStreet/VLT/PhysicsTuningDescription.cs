using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(PhysicsTuningDescription))]
    public class PhysicsTuningDescription : VLTBaseType
    {
        public PhysicsTuningDescription(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PhysicsTuningDescription(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RefSpec PhysicsTuning { get; set; }
        public bool Increase { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            PhysicsTuning = new RefSpec(Class, Field, Collection);
            PhysicsTuning.Read(vault, br);
            Increase = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            PhysicsTuning.Write(vault, bw);
            bw.Write(Increase);
            bw.AlignWriter(4);
        }
    }
}