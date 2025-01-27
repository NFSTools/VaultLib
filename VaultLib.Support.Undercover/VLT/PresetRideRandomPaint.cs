using System.IO;
using VaultLib.Core;
using VaultLib.Core.Types;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(PresetRideRandomPaint))]
public class PresetRideRandomPaint: VltBaseType<uint>
{
    public PresetRidePaint Paint { get; set; } = new();
    public float Chance { get; set; }

    public override void Read(VaultReadContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryReader br)
    {
        Paint.Read(context, fieldContext, br);
        Chance = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<uint> context, FieldReadWriteContext<uint> fieldContext, BinaryWriter bw)
    {
        Paint.Write(context, fieldContext, bw);
        bw.Write(Chance);
    }
}