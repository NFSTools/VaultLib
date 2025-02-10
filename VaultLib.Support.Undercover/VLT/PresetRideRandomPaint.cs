using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(PresetRideRandomPaint))]
public class PresetRideRandomPaint : VltBaseType<Core.DataInterfaces.Key32>
{
    public PresetRidePaint Paint { get; set; } = new();
    public float Chance { get; set; }

    public override void Read(VaultReadContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        Paint.Read(context, fieldContext, br);
        Chance = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<Core.DataInterfaces.Key32> context,
        FieldReadWriteContext<Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        Paint.Write(context, fieldContext, bw);
        bw.Write(Chance);
    }

    public override object Clone()
    {
        return new PresetRideRandomPaint
        {
            Chance = Chance,
            Paint = (PresetRidePaint)Paint.Clone(),
        };
    }
}