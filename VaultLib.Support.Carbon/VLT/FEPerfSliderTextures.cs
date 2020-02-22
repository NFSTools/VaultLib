using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Frameworks.Speed;

namespace VaultLib.Support.Carbon.VLT
{
    [VLTTypeInfo(nameof(FEPerfSliderTextures))]
    public class FEPerfSliderTextures : VLTBaseType
    {
        public FEPerfSliderTextures(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEPerfSliderTextures(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public eFEPartUpgradeLevels Level { get; set; }
        public uint Name1 { get; set; }
        public uint Name2 { get; set; }
        public uint Name3 { get; set; }
        public uint Logo1 { get; set; }
        public uint Logo2 { get; set; }
        public uint Logo3 { get; set; }
        public uint Logo4 { get; set; }
        public uint Logo5 { get; set; }
        public uint Logo6 { get; set; }
        public uint Logo7 { get; set; }
        public uint Logo8 { get; set; }
        public uint Logo9 { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            Level = br.ReadEnum<eFEPartUpgradeLevels>();
            Name1 = br.ReadUInt32();
            Name2 = br.ReadUInt32();
            Name3 = br.ReadUInt32();
            Logo1 = br.ReadUInt32();
            Logo2 = br.ReadUInt32();
            Logo3 = br.ReadUInt32();
            Logo4 = br.ReadUInt32();
            Logo5 = br.ReadUInt32();
            Logo6 = br.ReadUInt32();
            Logo7 = br.ReadUInt32();
            Logo8 = br.ReadUInt32();
            Logo9 = br.ReadUInt32();
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.WriteEnum(Level);
            bw.Write(Name1);
            bw.Write(Name2);
            bw.Write(Name3);
            bw.Write(Logo1);
            bw.Write(Logo2);
            bw.Write(Logo3);
            bw.Write(Logo4);
            bw.Write(Logo5);
            bw.Write(Logo6);
            bw.Write(Logo7);
            bw.Write(Logo8);
            bw.Write(Logo9);
        }
    }
}