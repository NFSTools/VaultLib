using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed.VLT
{
    [VltTypeInfo(nameof(PhysicsTuningSliderUnlock))]
    public class PhysicsTuningSliderUnlock : VltBaseType
    {
        public RefSpec PhysicsTuningSlider { get; set; } = new();
        public float Range { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            PhysicsTuningSlider.Read(context, fieldContext, br);
            Range = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            PhysicsTuningSlider.Write(context, fieldContext, bw);
            bw.Write(Range);
        }
    }
}