using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Frameworks.Speed
{
    [VLTTypeInfo(nameof(PhysicsTuningPreset))]
    public class PhysicsTuningPreset : VLTBaseType
    {
        public PhysicsTuningPreset(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            PhysicsTuningSlider = new RefSpec(Class, Field, Collection);
        }

        public RefSpec PhysicsTuningSlider { get; set; }
        public bool CenteredAroundPreset { get; set; }
        public float Position { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            PhysicsTuningSlider.Read(vault, br);
            CenteredAroundPreset = br.ReadBoolean();
            br.AlignReader(4);
            Position = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            PhysicsTuningSlider.Write(vault, bw);
            bw.Write(CenteredAroundPreset);
            bw.AlignWriter(4);
            bw.Write(Position);
        }
    }
}