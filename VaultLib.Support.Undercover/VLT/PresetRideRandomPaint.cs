using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(PresetRideRandomPaint))]
    public class PresetRideRandomPaint : VLTBaseType
    {
        public PresetRideRandomPaint(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PresetRideRandomPaint(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public PresetRidePaint Paint { get; set; }
        public float Chance { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Paint = new PresetRidePaint(Class, Field, Collection);
            Paint.Read(vault, br);
            Chance = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            Paint.Write(vault, bw);
            bw.Write(Chance);
        }
    }
}