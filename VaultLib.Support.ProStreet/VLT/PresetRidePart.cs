using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(PresetRidePart))]
    public class PresetRidePart : VLTBaseType
    {
        public PresetRidePart(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PresetRidePart(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public CAR_SLOT_ID SlotID { get; set; }
        public RefSpec Part { get; set; }
        public uint PartArrayIndex { get; set; }
        public uint KitNumber { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Part = new RefSpec(Class, Field, Collection);
            SlotID = br.ReadEnum<CAR_SLOT_ID>();
            Part.Read(vault, br);
            PartArrayIndex = br.ReadUInt32();
            KitNumber = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Part.Write(vault, bw);
            bw.Write(PartArrayIndex);
            bw.Write(KitNumber);
        }
    }
}