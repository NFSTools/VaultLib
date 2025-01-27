using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FEVinylRegionData))]
public class FEVinylRegionData: VltBaseType<uint>
{
    public uint HAL_ID { get; set; }
    public RefSpec<uint> Camera { get; set; } = new();

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        HAL_ID = br.ReadUInt32();
        Camera.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        bw.Write(HAL_ID);
        Camera.Write(context, fieldContext, bw);
    }
}