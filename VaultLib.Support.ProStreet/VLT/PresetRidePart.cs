using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT
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
        public uint PartArrayIndex { get; set; }
        public uint KitNumber { get; set; }

        public override void Read(VaultLoadContext context, BinaryReader br)
        {
            SlotID = br.ReadEnum<CAR_SLOT_ID>();
            Part.Read(context, br);
            PartArrayIndex = br.ReadUInt32();
            KitNumber = br.ReadUInt32();
        }

        public override void Write(VaultSaveContext context, BinaryWriter bw)
        {
            bw.WriteEnum(SlotID);
            Part.Write(context, bw);
            bw.Write(PartArrayIndex);
            bw.Write(KitNumber);
        }
    }
}