using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(PresetRideAutosculpt))]
    public class PresetRideAutosculpt : VLTBaseType
    {
        public PresetRideAutosculpt(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public PresetRideAutosculpt(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public PresetRideAutosculptRegion RegionID { get; set; }
        public float SculptZone0 { get; set; }
        public float SculptZone1 { get; set; }
        public float SculptZone2 { get; set; }
        public float SculptZone3 { get; set; }
        public float SculptZone4 { get; set; }
        public float SculptZone5 { get; set; }
        public float SculptZone6 { get; set; }
        public float SculptZone7 { get; set; }
        public float SculptZone8 { get; set; }
        public float SculptZone9 { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            RegionID = br.ReadEnum<PresetRideAutosculptRegion>();
            SculptZone0 = br.ReadSingle();
            SculptZone1 = br.ReadSingle();
            SculptZone2 = br.ReadSingle();
            SculptZone3 = br.ReadSingle();
            SculptZone4 = br.ReadSingle();
            SculptZone5 = br.ReadSingle();
            SculptZone6 = br.ReadSingle();
            SculptZone7 = br.ReadSingle();
            SculptZone8 = br.ReadSingle();
            SculptZone9 = br.ReadSingle();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(RegionID);
            bw.Write(SculptZone0);
            bw.Write(SculptZone1);
            bw.Write(SculptZone2);
            bw.Write(SculptZone3);
            bw.Write(SculptZone4);
            bw.Write(SculptZone5);
            bw.Write(SculptZone6);
            bw.Write(SculptZone7);
            bw.Write(SculptZone8);
            bw.Write(SculptZone9);
        }
    }
}