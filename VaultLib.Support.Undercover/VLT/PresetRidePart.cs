using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(PresetRidePart))]
    public class PresetRidePart : VltBaseType
    {
        public PresetRidePart(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Part = new RefSpec(Class, Field, Collection);
        }

        public CAR_SLOT_ID SlotID { get; set; }
        public RefSpec Part { get; set; }
        public uint KitNumber { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            SlotID = br.ReadEnum<CAR_SLOT_ID>();
            Part.Read(context, fieldContext, br);
            KitNumber = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Part.Write(context, fieldContext, bw);
            bw.Write(KitNumber);
        }
    }
}