using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Core.Utils;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.Undercover.VLT;

[VltTypeInfo(nameof(PresetRidePaint))]
public class PresetRidePaint: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public ePaintSlot SlotID { get; set; }
    public RefSpec32 Group { get; set; } = new();
    public byte Swatch { get; set; }
    public float Saturation { get; set; }
    public float Variance { get; set; }
    public bool Unknown { get; set; }

    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        SlotID = br.ReadEnum<ePaintSlot>();
        Group.Read(context, fieldContext, br);
        Swatch = br.ReadByte();
        br.SafeAlignReader(4);
        Saturation = br.ReadSingle();
        Variance = br.ReadSingle();
        Unknown = br.ReadBoolean();
        br.SafeAlignReader(4);
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(SlotID);
        Group.Write(context, fieldContext, bw);
        bw.Write(Swatch);
        bw.AlignWriter(4);
        bw.Write(Saturation);
        bw.Write(Variance);
        bw.Write(Unknown);
        bw.AlignWriter(4);
    }
}