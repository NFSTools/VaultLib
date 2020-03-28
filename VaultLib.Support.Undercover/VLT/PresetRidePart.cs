using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(PresetRidePart))]
    public class PresetRidePart : VLTBaseType
    {
        public PresetRidePart(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Part = new RefSpec(Class, Field, Collection);
        }

        public CAR_SLOT_ID SlotID { get; set; }
        public RefSpec Part { get; set; }
        public uint KitNumber { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            SlotID = br.ReadEnum<CAR_SLOT_ID>();
            Part.Read(vault, br);
            KitNumber = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Part.Write(vault, bw);
            bw.Write(KitNumber);
        }
    }
}