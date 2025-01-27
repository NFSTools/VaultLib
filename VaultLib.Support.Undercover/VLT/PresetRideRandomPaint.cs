using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(PresetRideRandomPaint))]
public class PresetRideRandomPaint: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public PresetRidePaint Paint { get; set; } = new();
    public float Chance { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Paint.Read(context, fieldContext, br);
        Chance = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Paint.Write(context, fieldContext, bw);
        bw.Write(Chance);
    }
}