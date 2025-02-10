using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(FEVinylRegionData))]
public class FEVinylRegionData : VltBaseType<Core.DataInterfaces.Key32>
{
    public uint HAL_ID { get; set; }
    public RefSpec32 Camera { get; set; } = new();

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        HAL_ID = br.ReadUInt32();
        Camera.Read(context, fieldContext, br);
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.Write(HAL_ID);
        Camera.Write(context, fieldContext, bw);
    }

    public override object Clone()
    {
        return new FEVinylRegionData
        {
            HAL_ID = HAL_ID,
            Camera = (RefSpec32)Camera.Clone(),
        };
    }
}