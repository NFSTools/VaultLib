using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PhysicsTuningDescription))]
    public class PhysicsTuningDescription : VltBaseType
    {
        public PhysicsTuningDescription(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            PhysicsTuning = new RefSpec(Class, Field, Collection);
        }

        public RefSpec PhysicsTuning { get; set; }
        public bool Increase { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PhysicsTuning.Read(context, fieldContext, br);
            Increase = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            PhysicsTuning.Write(context, fieldContext, bw);
            bw.Write(Increase);
            bw.AlignWriter(4);
        }
    }
}