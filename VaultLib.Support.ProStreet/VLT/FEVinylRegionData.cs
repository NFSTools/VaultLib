using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT
{
    [VLTTypeInfo(nameof(FEVinylRegionData))]
    public class FEVinylRegionData : VLTBaseType
    {
        public FEVinylRegionData(VltClass @class, VltClassField field, VltCollection collection) : base(@class, field, collection)
        {
        }

        public FEVinylRegionData(VltClass @class, VltClassField field) : base(@class, field)
        {
        }

        public uint HAL_ID { get; set; }
        public RefSpec Camera { get; set; }

        public override void Read(Vault vault, BinaryReader br)
        {
            HAL_ID = br.ReadUInt32();
            Camera = new RefSpec(Class, Field, Collection);
            Camera.Read(vault, br);
        }

        public override void Write(Vault vault, BinaryWriter bw)
        {
            bw.Write(HAL_ID);
            Camera.Write(vault, bw);
        }
    }
}