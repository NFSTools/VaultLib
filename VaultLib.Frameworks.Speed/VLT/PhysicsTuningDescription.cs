using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VLTTypeInfo(nameof(PhysicsTuningDescription))]
    public class PhysicsTuningDescription : VLTBaseType
    {
        public PhysicsTuningDescription(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            PhysicsTuning = new RefSpec(Class, Field, Collection);
        }

        public RefSpec PhysicsTuning { get; set; }
        public bool Increase { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            PhysicsTuning.Read(context, br);
            Increase = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            PhysicsTuning.Write(context, bw);
            bw.Write(Increase);
            bw.AlignWriter(4);
        }
    }
}