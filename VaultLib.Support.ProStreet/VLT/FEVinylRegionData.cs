using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FEVinylRegionData))]
public class FEVinylRegionData : VltBaseType
{
    public uint HAL_ID { get; set; }
    public RefSpec Camera { get; set; } = new();

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