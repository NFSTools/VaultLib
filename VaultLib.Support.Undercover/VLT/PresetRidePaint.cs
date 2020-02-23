using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(PresetRidePaint))]
    public class PresetRidePaint : VLTBaseType
    {
        public PresetRidePaint(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PresetRidePaint(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public ePaintSlot SlotID { get; set; }
        public RefSpec Group { get; set; }
        public byte Swatch { get; set; }
        public float Saturation { get; set; }
        public float Variance { get; set; }
        public bool Unknown { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Group = new RefSpec(Class, Field, Collection);
            SlotID = br.ReadEnum<ePaintSlot>();
            Group.Read(vault, br);
            Swatch = br.ReadByte();
            br.AlignReader(4);
            Saturation = br.ReadSingle();
            Variance = br.ReadSingle();
            Unknown = br.ReadBoolean();
            br.AlignReader(4);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Group.Write(vault, bw);
            bw.Write(Swatch);
            bw.AlignWriter(4);
            bw.Write(Saturation);
            bw.Write(Variance);
            bw.Write(Unknown);
            bw.AlignWriter(4);
        }
    }
}