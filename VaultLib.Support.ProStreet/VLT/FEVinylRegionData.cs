using System.IO;
using VaultLib.Core;
using VaultLib.Core.Data;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT
{
    [VltTypeInfo(nameof(FEVinylRegionData))]
    public class FEVinylRegionData : VltBaseType
    {
        public FEVinylRegionData(VltClass @class, VltClassField field, VltCollection collection = null) : base(@class, field, collection)
        {
            Camera = new RefSpec(Class, Field, Collection);
        }

        public uint HAL_ID { get; set; }
        public RefSpec Camera { get; set; }

        public override void Read(VaultReadContext context, FieldReadWriteContext fieldContext, BinaryReader br)
        {
            HAL_ID = br.ReadUInt32();
            Camera.Read(context, fieldContext, br);
        }

        public override void Write(VaultWriteContext context, FieldReadWriteContext fieldContext, BinaryWriter bw)
        {
            bw.Write(HAL_ID);
            Camera.Write(context, fieldContext, bw);
        }
    }
}