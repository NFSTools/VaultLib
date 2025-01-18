using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(PresetRidePaint))]
    public class PresetRidePaint : VltBaseType
    {
        public PresetRidePaint(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Group = new RefSpec(Class, Field, Collection);
            Swatch = new RefSpec(Class, Field, Collection);
        }

        public ePaintSlot SlotID { get; set; }
        public RefSpec Group { get; set; }
        public RefSpec Swatch { get; set; }
        public uint KitNumber { get; set; }
        public float Saturation { get; set; }
        public float Variance { get; set; }
        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            SlotID = br.ReadEnum<ePaintSlot>();
            Group.Read(context, fieldContext, br);
            Swatch.Read(context, fieldContext, br);
            KitNumber = br.ReadUInt32();
            Saturation = br.ReadSingle();
            Variance = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Group.Write(context, fieldContext, bw);
            Swatch.Write(context, fieldContext, bw);
            bw.Write(KitNumber);
            bw.Write(Saturation);
            bw.Write(Variance);
        }
    }
}