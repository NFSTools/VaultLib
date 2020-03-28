using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT
{
    [VLTTypeInfo(nameof(RoadblockSetup))]
    public class RoadblockSetup : VLTBaseType
    {
        public RoadblockSetup(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Contents = new RoadblockElement[6];
            for (int i = 0; i < 6; i++)
                Contents[i] = new RoadblockElement(Class, Field, Collection);
        }

        public float MinimumWidthRequired { get; set; }
        public uint RequiredVehicles { get; set; }
        public float MinimumThreatLevel { get; set; }
        public float MaximumThreatLevel { get; set; }
        public RoadblockElement[] Contents { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            MinimumWidthRequired = br.ReadSingle();
            RequiredVehicles = br.ReadUInt32();
            MinimumThreatLevel = br.ReadSingle();
            MaximumThreatLevel = br.ReadSingle();

            for (int i = 0; i < 6; i++)
            {
                Contents[i].Read(vault, br);
            }
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(MinimumWidthRequired);
            bw.Write(RequiredVehicles);
            bw.Write(MinimumThreatLevel);
            bw.Write(MaximumThreatLevel);

            for (int i = 0; i < 6; i++)
            {
                Contents[i].Write(vault, bw);
            }
        }
    }
}