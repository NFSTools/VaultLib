using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(PhysicsTuningSliderUnlock))]
    public class PhysicsTuningSliderUnlock : VLTBaseType
    {
        public PhysicsTuningSliderUnlock(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PhysicsTuningSliderUnlock(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public RefSpec PhysicsTuningSlider { get; set; }
        public float Range { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            PhysicsTuningSlider = new RefSpec(Class, Field, Collection);
            PhysicsTuningSlider.Read(vault, br);
            Range = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            PhysicsTuningSlider.Write(vault, bw);
            bw.Write(Range);
        }
    }
}