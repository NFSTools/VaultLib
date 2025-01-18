using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PhysicsTuningSliderUnlock))]
    public class PhysicsTuningSliderUnlock : VltBaseType
    {
        public PhysicsTuningSliderUnlock(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            PhysicsTuningSlider = new RefSpec(Class, Field, Collection);
        }

        public RefSpec PhysicsTuningSlider { get; set; }
        public float Range { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            PhysicsTuningSlider.Read(context, br);
            Range = br.ReadSingle();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            PhysicsTuningSlider.Write(context, bw);
            bw.Write(Range);
        }
    }
}