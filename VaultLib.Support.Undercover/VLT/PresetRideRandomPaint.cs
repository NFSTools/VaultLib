using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VltTypeInfo(nameof(PresetRideRandomPaint))]
    public class PresetRideRandomPaint : VltBaseType
    {
        public PresetRideRandomPaint(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Paint = new PresetRidePaint(Class, Field, Collection);
        }

        public PresetRidePaint Paint { get; set; }
        public float Chance { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            Paint.Read(context, fieldContext, br);
            Chance = br.ReadSingle();
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            Paint.Write(context, fieldContext, bw);
            bw.Write(Chance);
        }
    }
}