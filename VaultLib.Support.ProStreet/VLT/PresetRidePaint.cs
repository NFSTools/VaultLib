using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT
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
        public RefSpec Swatch { get; set; }
        public uint KitNumber { get; set; }
        public float Saturation { get; set; }
        public float Variance { get; set; }
        public override void Read(Vault vault, BinaryReader br)
        {
            Group = new RefSpec(Class, Field, Collection);
            Swatch = new RefSpec(Class, Field, Collection);

            SlotID = br.ReadEnum<ePaintSlot>();
            Group.Read(vault, br);
            Swatch.Read(vault, br);
            KitNumber = br.ReadUInt32();
            Saturation = br.ReadSingle();
            Variance = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Group.Write(vault, bw);
            Swatch.Write(vault, bw);
            bw.Write(KitNumber);
            bw.Write(Saturation);
            bw.Write(Variance);
        }
    }
}