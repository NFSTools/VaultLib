using System.IO;
using CoreLibraries.IO;
using VaultLib.Core;
using VaultLib.Core.Types;
using VaultLib.Core.Types.Attrib;
using VaultLib.Frameworks.Speed.VLT;

namespace VaultLib.Support.ProStreet.VLT;

[VltTypeInfo(nameof(PresetRidePaint))]
public class PresetRidePaint: VltBaseType<VaultLib.Core.DataInterfaces.Key32>
{
    public ePaintSlot SlotID { get; set; }
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> Group { get; set; } = new();
    public RefSpec<VaultLib.Core.DataInterfaces.Key32> Swatch { get; set; } = new();
    public uint KitNumber { get; set; }
    public float Saturation { get; set; }
    public float Variance { get; set; }
    public override void Read(VaultReadContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryReader br)
    {
        SlotID = br.ReadEnum<ePaintSlot>();
        Group.Read(context, fieldContext, br);
        Swatch.Read(context, fieldContext, br);
        KitNumber = br.ReadUInt32();
        Saturation = br.ReadSingle();
        Variance = br.ReadSingle();
    }

    public override void Write(VaultWriteContext<VaultLib.Core.DataInterfaces.Key32> context, FieldReadWriteContext<VaultLib.Core.DataInterfaces.Key32> fieldContext, BinaryWriter bw)
    {
        bw.WriteEnum(SlotID);
        Group.Write(context, fieldContext, bw);
        Swatch.Write(context, fieldContext, bw);
        bw.Write(KitNumber);
        bw.Write(Saturation);
        bw.Write(Variance);
    }
}