using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(PresetRidePart))]
    public class PresetRidePart : VltBaseType
    {
        public CAR_SLOT_ID SlotID { get; set; }
        public RefSpec Part { get; set; } = new();
        public uint PartArrayIndex { get; set; }
        public uint KitNumber { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            SlotID = br.ReadEnum<CAR_SLOT_ID>();
            Part.Read(context, fieldContext, br);
            PartArrayIndex = br.ReadUInt32();
            KitNumber = br.ReadUInt32();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Part.Write(context, fieldContext, bw);
            bw.Write(PartArrayIndex);
            bw.Write(KitNumber);
        }
    }
}